using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace GameJam
{
    public partial class Player
    {
        void Idle_Enter()
        {
            Debug.Log("Player State : " + fsm.State);
        }
        void Idle_Move(float direction)
        {
            fsm.ChangeState(States.Move);
            fsm.Driver.Move?.Invoke(direction);
        }
        void Idle_Dash(float direction)
        {
            fsm.ChangeState(States.Dash);
            fsm.Driver.Dash?.Invoke(direction);
        }
        void Idle_Drop()
        {

        }
        void Idle_Jump(float force)
        {
            fsm.ChangeState(States.Jump);
            fsm.Driver.Jump?.Invoke(force);
        }
        void Idle_Update()
        {
            if (inputData.x != 0) {
                Move(inputData.x);
                return;
            }

            if (isShift && CanDash) {
                Dash(1);
            }
            else if (isSpace) {
                if (inputData.y < 0) {
                    Debug.Log("Space Pressed With Down Key");
                    Drop();
                }
                else if (CanJump) {
                    Debug.Log("Space Pressed");
                    Jump(jumpForce);
                }
            }
        }
        void Idle_FixedUpdate()
        {
            if(rb2D.velocity.x > 0) {
                rb2D.velocity -= new Vector2(moveSpeed * moveDeltaTime, 0);
                if (rb2D.velocity.x < 0) rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            }
            else if (rb2D.velocity.x < 0) {
                rb2D.velocity += new Vector2(moveSpeed * moveDeltaTime, 0);
                if (rb2D.velocity.x > 0) rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            }
            if (!isGrounded) {
                // 만약 넉백 등의 이유로 밀려서 떨어지면 점프 상태로 변경해야 한다
                Jump(0);
            }
        }

        private float moveDirection;
        void Move_Enter()
        {
            Debug.Log("Player State : " + fsm.State);
        }
        void Move_Idle()
        {
            fsm.ChangeState(States.Idle);
        }
        void Move_Move(float direction)
        {
            moveDirection = direction;
        }
        void Move_Dash(float direction)
        {
            fsm.ChangeState(States.Dash);
            fsm.Driver.Dash?.Invoke(direction);
        }
        void Move_Jump(float force)
        {
            fsm.ChangeState(States.Jump);
            fsm.Driver.Jump?.Invoke(force);
        }
        void Move_Update()
        {
            if (inputData.x != 0) {
                if (isGrounded)
                    Move(inputData.x);
            }
            else {
                // 입력이 없으면 Idle 상태로 전환해본다
                if (isGrounded) {
                    Idle();
                    return;
                }
                // 땅에 있지 않다면 Idle 상태로 전환하지 않는다
            }

            if (isShift && CanDash) {
                Debug.Log("Shift Pressed");
                Dash(inputData.x);
            }
            else if (isSpace) {
                if (inputData.y < 0) {
                    Debug.Log("Space Pressed With Down Key");
                    Drop();
                }
                else if (CanJump) {
                    Debug.Log("Space Pressed");
                    Jump(jumpForce);
                }
            }
        }
        void Move_FixedUpdate()
        {
            rb2D.velocity += new Vector2(moveDirection * moveSpeed * moveDeltaTime, 0);

            if (moveDirection > 0 &&
                    rb2D.velocity.x > moveSpeed) {
                // 움직이는 방향이 오른쪽일 경우, 최대 속도는 +moveSpeed
                rb2D.velocity = new Vector2(moveSpeed, rb2D.velocity.y);
            }

            if (moveDirection < 0 &&
                rb2D.velocity.x < -moveSpeed) {
                // 움직이는 방향이 왼쪽일 경우, 최대 속도는 -moveSpeed
                rb2D.velocity = new Vector2(-moveSpeed, rb2D.velocity.y);
            }

            if (!isGrounded) {
                // 만약 넉백 등의 이유로 밀려서 떨어지면 점프 상태로 변경해야 한다
                Jump(0);
            }
        }

        private bool isDash = false;
        private bool isEffect = false;
        private float dashCooldown = 0;
        public float dashDistance = 5f;
        public float dashCoolTime = 4f;
        public float dashMotionTime = 0.5f;
        public float dashEffectTime = 0.33f;
        private bool CanDash => dashCooldown <= 0f;
        Coroutine dashRoutine;
        void Dash_Enter()
        {
            Debug.Log("Player State : " + fsm.State);
        }
        void Dash_Finally()
        {
            isDash = false;
            isEffect = false;
            if (dashRoutine != null)
                StopCoroutine(dashRoutine);
        }
        void Dash_Dash(float direction)
        {
            dashCooldown = dashCoolTime;
            isDash = true;
            isEffect = true;
            dashRoutine = StartCoroutine(DashRoutine());
            IEnumerator DashRoutine()
            {
                Debug.Log("Dash Start");

                float deltaTime = 0f;
                //float deltaDistance = 
                while (true) {
                    yield return new WaitForFixedUpdate();
                    deltaTime += Time.fixedDeltaTime;
                    if (deltaTime > dashEffectTime && isEffect) isEffect = false;
                    if (deltaTime > dashMotionTime) break;
                }
                Debug.Log("Dash End");
                isDash = false;
                dashRoutine = null;
                if (isGrounded) {
                    if (inputData.x != 0) Move(inputData.x);
                    else Idle();
                }
                else {
                    fsm.ChangeState(States.Jump);   // 공중에 있을 경우 그냥 점프 상태로 변환
                }
            }
        }
        void Dash_Idle()
        {
            Debug.Log("Dash To Idle");
            fsm.ChangeState(States.Idle);
        }
        void Dash_Move(float direction)
        {
            Debug.Log("Dash To Move");
            fsm.ChangeState(States.Move);
            fsm.Driver.Move?.Invoke(direction);
        }

        Coroutine jumpRoutine;
        public int jumpCounter = 0;
        public int maxJumpCounter = 2;
        public int jumpDashCounter;
        private bool CanJump => jumpCounter < maxJumpCounter;
        private bool CanDashWhenJump => jumpDashCounter > 0;
        void Jump_Enter()
        {
            Debug.Log("Player State : " + fsm.State);
            if (jumpCounter == 0) jumpDashCounter = 1;
        }
        void Jump_Finally()
        {
            if (jumpRoutine != null) {
                StopCoroutine(jumpRoutine);
            }
            if (fsm.NextState != States.Dash) {
                jumpCounter = 0;
                jumpRoutine = null;
            }
        }
        void Jump_Jump(float force)
        {
            Debug.Log("Checker");
            
            if (force > 0) {
                rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
                rb2D.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            }
            jumpCounter++;
            jumpRoutine = StartCoroutine(JumpRoutine());

            IEnumerator JumpRoutine()
            {
                yield return new WaitUntil(() => !isGrounded);
                yield return new WaitUntil(() => isGrounded);
                //yield return new WaitUntil(() => rb2D.velocity.y < 0);
                jumpCounter = 0;
                jumpRoutine = null;

                if (inputData.x != 0) Move(inputData.x);
                else Idle();
            }
        }
        void Jump_Idle()
        {
            Debug.Log("Jump To Idle");
            fsm.ChangeState(States.Idle);
        }
        void Jump_Move(float direction)
        {
            Debug.Log("Jump To Move");
            fsm.ChangeState(States.Move);
            fsm.Driver.Move?.Invoke(direction);
        }
        void Jump_Dash(float direction)
        {
            Debug.Log("Jump To Dash");
            fsm.ChangeState(States.Dash);
            fsm.Driver.Dash?.Invoke(direction);
        }
        void Jump_Update()
        {
            if (isSpace && CanJump) {
                Jump(jumpForce);
            }
            else if(isShift && CanDashWhenJump) {
                if(inputData.x != 0) {
                    Dash(inputData.x);
                }
                else {
                    if (rb2D.velocity.x > 0) Dash(1);
                    else Dash(-1);
                }
            }
        }

        void Dead_Enter()
        {
            Debug.Log("Player State : " + fsm.State);
        }
        void Idle_Dead()
        {
            fsm.ChangeState(States.Dead, StateTransition.Overwrite);
        }
        void Move_Dead()
        {
            fsm.ChangeState(States.Dead, StateTransition.Overwrite);
        }
        void Dash_Dead()
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
