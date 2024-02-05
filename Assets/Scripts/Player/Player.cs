using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    [RequireComponent(typeof(Rigidbody2D))]
    public partial class Player : MonoBehaviour
    {
        public static Player instance;
        public Rigidbody2D rb2D;
        public Collider2D col2D;

        public enum States
        {
            Idle,
            Move,
            Jump,
            Dash,
            Attack,
            Hit,
            Dead
        }

        public class Driver
        {
            public StateEvent Idle;
            public StateEvent<float> Move;
            public StateEvent<float> Jump;
            public StateEvent<float> Dash;
            public StateEvent Attack;
            public StateEvent Hit;
            public StateEvent Dead;
            public StateEvent Update;
            public StateEvent FixedUpdate;
        }

        public float moveSpeed = 5f;
        public float moveAccel = 0.05f;     // 몇 초 만에 최대 속도에 도달하는가?
        public float jumpForce = 5f;

        private float moveDeltaTime;

        private Vector2 inputData;
        private bool isShift;
        private bool isSpace;

        public bool isGrounded;
        public Collider2D groundChecker;
        public ContactFilter2D groundFilter;
        private List<Collider2D> groundData;

        private StateMachine<States, Driver> fsm;

        public void Idle() => fsm.Driver.Idle?.Invoke();
        public void Move(float direction) => fsm.Driver.Move?.Invoke(direction);
        public void Jump(float force) => fsm.Driver.Jump?.Invoke(force);
        public void Dash(float direction) => fsm.Driver.Dash?.Invoke(direction);
        public void Attack() => fsm.Driver.Attack?.Invoke();
        public void Hit() => fsm.Driver.Hit?.Invoke();
        public void Dead() => fsm.Driver.Dead?.Invoke();

        void Awake()
        {
            Player.instance = this;
            moveDeltaTime = (1 / moveAccel) / 50;
            rb2D = GetComponent<Rigidbody2D>();
            col2D = GetComponent<Collider2D>();
            groundData = new List<Collider2D>();

            fsm = new StateMachine<States, Driver>(this);
            fsm.ChangeState(States.Idle);
        }

        private void Update()
        {
            GetInputKey();
            fsm.Driver.Update?.Invoke();
            if (dashCooldown > 0) dashCooldown -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            isGrounded = Physics2D.OverlapCollider(groundChecker, groundFilter, groundData) > 0;
            fsm.Driver.FixedUpdate?.Invoke();
        }

        void GetInputKey()
        {
            isSpace = false;
            isShift = false;
            inputData = Vector2.zero;
            if (Input.GetKey(KeyCode.A))
                inputData.x -= 1;
            if (Input.GetKey(KeyCode.D))
                inputData.x += 1;
            if (Input.GetKey(KeyCode.S)) 
                inputData.y -= 1;
            if (Input.GetKey(KeyCode.W)) 
                inputData.y += 1;
            if (Input.GetKeyDown(KeyCode.LeftShift))
                isShift = true;
            if (Input.GetKeyDown(KeyCode.Space))
                isSpace = true;
        }
    }
}
