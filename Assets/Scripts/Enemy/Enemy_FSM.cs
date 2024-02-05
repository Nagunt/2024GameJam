using MonsterLove.StateMachine;
using System.Collections;
using UnityEngine;

namespace GameJam
{
    public partial class Enemy
    {
        private float moveDirection;

        void Idle_Move(Vector2 direction)
        {
            moveDirection = direction.x;
            fsm.ChangeState(States.Move, StateTransition.Overwrite);
        }

        void Idle_Attack()
        {
            fsm.ChangeState(States.Attack, StateTransition.Overwrite);
        }

        void Idle_Hit()
        {
            fsm.ChangeState(States.Hit, StateTransition.Overwrite);
        }

        void Move_Enter()
        {
            
        }

        void Move_Exit()
        {
            moveDirection = 0;
            isJump = false;
            jumpRoutine = null;
        }

        void Move_Jump()
        {
            // 점프는 Move 상태에서만 가능하다
            if (isGrounded && !isJump) {
                isJump = true;
            }
        }
        Coroutine jumpRoutine;
        void Move_FixedUpdate()
        {
            if (rb2D) {
                rb2D.velocity = new Vector2(moveDirection * moveSpeed, rb2D.velocity.y);
                if (isJump && jumpRoutine == null) {
                    jumpRoutine = StartCoroutine(JumpRoutine());
                }
            }

            IEnumerator JumpRoutine()
            {
                rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                yield return new WaitUntil(() => isGrounded == false);
                isJump = false;
                jumpRoutine = null;
            }
        }

        void Attack_Enter()
        {


        }

        void Attack_Exit()
        {

        }

        void Attack_Finally()
        {

        }

        void Hit_Enter()
        {

        }

        void Hit_Exit()
        {

        }

        void Hit_Finally()
        {

        }

        void Dead_Enter()
        {

        }

        void Idle_Dead()
        {
            fsm.ChangeState(States.Dead, StateTransition.Overwrite);
        }
        void Move_Dead()
        {
            fsm.ChangeState(States.Dead, StateTransition.Overwrite);
        }
        void Attack_Dead()
        {
            fsm.ChangeState(States.Dead, StateTransition.Overwrite);
        }
        void Hit_Dead()
        {
            fsm.ChangeState(States.Dead, StateTransition.Overwrite);
        }
    }
}
