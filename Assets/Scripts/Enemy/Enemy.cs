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
            public StateEvent FixedUpdate;
        }

        private StateMachine<States, Driver> fsm; 

        public bool isJump;
        public bool isGrounded;
        private float groundDistance = 0.05f;
        private int groundMask;

        public float moveSpeed = 5f;
        public float jumpForce = 5f;

        private Rigidbody2D rb2D;

        public Collider2D searchArea;
        public ContactFilter2D searchFilter;
        private List<Collider2D> searchResults = new List<Collider2D>();

        public Transform target;

        private Vector2 firstPosition;
        public void Idle() => fsm.Driver.Idle?.Invoke();
        public void Move(Vector2 direction) => fsm.Driver.Move?.Invoke(direction);
        public void Jump() => fsm.Driver.Jump?.Invoke();
        public void Attack() => fsm.Driver.Attack?.Invoke();
        public void Hit() => fsm.Driver.Hit?.Invoke();
        public void Dead() => fsm.Driver.Dead?.Invoke();

        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
            groundMask = LayerMask.GetMask("Ground");
            firstPosition = transform.position;

            fsm = new StateMachine<States, Driver>(this);
            fsm.ChangeState(States.Idle);
        }

        private void FixedUpdate()
        {
            isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);
            if (searchArea) {
                searchResults.Clear();
                //searchArea가 없으면 플레이어를 찾지 않는다
                if (Physics2D.OverlapCollider(searchArea, searchFilter, searchResults) > 0) {
                    foreach (Collider2D col2D in searchResults) {
                        if (col2D.CompareTag("Player")) {
                            target = col2D.transform;
                        }
                    }
                }
                else {
                    // searchArea 밖에 플레이어가 나가면 타겟을 초기화한다
                    target = null;
                }
            }
            fsm.Driver.FixedUpdate?.Invoke();
        }
    }
}