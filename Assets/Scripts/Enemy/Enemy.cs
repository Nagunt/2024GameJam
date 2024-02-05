using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

namespace GameJam
{
    [RequireComponent(typeof(Rigidbody2D))]
    public partial class Enemy : MonoBehaviour
    {
        public enum States
        {
            Idle,
            Move,
            Attack,
            Hit,
            Dead
        }

        public class Driver
        {
            public StateEvent Idle;
            public StateEvent<Vector2> Move;
            public StateEvent Jump;
            public StateEvent Attack;
            public StateEvent Hit;
            public StateEvent Dead;
            public StateEvent Update;
            public StateEvent FixedUpdate;
        }

        private StateMachine<States, Driver> fsm; 

        public bool isJump;
        public bool isGrounded;
        private float groundDistance = 0.05f;
        private int groundMask;

        public float moveSpeed = 5f;
        public float moveAccel = 0.05f;     // 몇 초 만에 최대 속도에 도달하는가?
        public float jumpForce = 5f;

        private float moveDeltaTime;

        private Rigidbody2D rb2D;

        public Transform target;

        private Vector2 firstPosition;

        private Vector2 destination;
        public void Idle() => fsm.Driver.Idle?.Invoke();
        public void Move(Vector2 direction) => fsm.Driver.Move?.Invoke(direction);
        public void Jump() => fsm.Driver.Jump?.Invoke();
        public void Attack() => fsm.Driver.Attack?.Invoke();
        public void Hit() => fsm.Driver.Hit?.Invoke();
        public void Dead() => fsm.Driver.Dead?.Invoke();

        public void SetTarget(Transform target) => this.target = target;

        public void SetDestination(Vector2 destination) => this.destination = destination;

        public Graphic graphic;

        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
            graphic = GetComponentInChildren<Graphic>();
            groundMask = LayerMask.GetMask("Ground");
            firstPosition = transform.position;
            moveDeltaTime = (1 / moveAccel) / 50;

            fsm = new StateMachine<States, Driver>(this);
            fsm.ChangeState(States.Idle);
        }

        private void Update()
        {
            fsm.Driver.Update?.Invoke();
        }

        private void FixedUpdate()
        {
            isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);
            fsm.Driver.FixedUpdate?.Invoke();
            graphic.SetMoveDirection(moveDirection);
        }
    }
}