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
        private Rigidbody2D rb2D;

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
            public StateEvent Drop;
            public StateEvent Attack;
            public StateEvent Hit;
            public StateEvent Dead;
            public StateEvent Update;
            public StateEvent FixedUpdate;
        }






        ////public State state;
        //public bool isOnGround;
        //public bool isOnAirGround;
        ////public bool isJump;
        //public bool isStickWall;
        //public bool isPressS;
        ////public bool isDash;
        //public bool isFall;

        //public Vector2 playerVelocity;
        //public float velX;
        //public float addA;
        //public float addD;
        //public float velY;
        //public int doubleJumpInt;

        
        //public float dashCooltime;
        //public float dashVel;
        //public bool isPressA;
        //public bool isPressD;

        //public float mass;

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
        public void Drop() => fsm.Driver.Drop?.Invoke();
        public void Dash(float direction) => fsm.Driver.Dash?.Invoke(direction);
        public void Attack() => fsm.Driver.Attack?.Invoke();
        public void Hit() => fsm.Driver.Hit?.Invoke();
        public void Dead() => fsm.Driver.Dead?.Invoke();

        void Awake()
        {
            Player.instance = this;
            moveDeltaTime = (1 / moveAccel) / 50;
            rb2D = GetComponent<Rigidbody2D>();
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

        //// Update is called once per frame
        //void Update()
        //{
        //    playerVelocity = rb2D.velocity;
        //    mass = rb2D.mass;
        //    velX = -addA + addD;
        //    if(playerVelocity.y>0)
        //    {
        //        isJump = true;
        //    }
        //    else
        //    {
        //        isJump = false;
        //        if(isPressS==false)
        //        {
        //            //AirGround.instance.tilemapCollider2D.isTrigger = false;
        //            AirGround.instance.compositeCollider2D.isTrigger = false;
        //        }

        //    }
        //    rb2D.velocity = new Vector2(velX, playerVelocity.y);
        //    dashCooltime -= Time.deltaTime;



        //    GetInputKey();
        //}

        //void GetInputKey()
        //{
        //    if(Input.GetKeyDown(KeyCode.W))
        //    {

        //    }
        //    if (Input.GetKey(KeyCode.A))
        //    {

        //        if(isOnGround==true&&isStickWall==true)
        //        {
        //            //addA = 3;
        //            rb2D.AddForce(new Vector2(-10, 0), ForceMode2D.Impulse);
        //        }
        //        else if(isOnGround==true&&isStickWall==false)
        //        {
        //            //addA = 3;
        //            rb2D.AddForce(new Vector2(-10, 0), ForceMode2D.Impulse);
        //        }
        //        else if (isOnGround == false && isStickWall == true)
        //        {
        //            //addA = 0;
        //        }
        //        else if (isOnGround == false && isStickWall == false)
        //        {
        //            //addA = 3;
        //            rb2D.AddForce(new Vector2(-10, 0), ForceMode2D.Impulse);
        //        }

        //        //rigidbody2D.AddForce(new Vector2(-10, 0), ForceMode2D.Impulse);
        //        isPressA = true;
        //    }
        //    else
        //    {
        //        addA = 0;
        //        isPressA = false;
        //    }
        //    if (Input.GetKey(KeyCode.S))
        //    {
        //        isPressS = true;
        //        StopCoroutine(PressSCoroutine());
        //    }
        //    if (Input.GetKeyUp(KeyCode.S))
        //    {
        //        StartCoroutine(PressSCoroutine());
        //    }
        //    if (Input.GetKey(KeyCode.D))
        //    {
        //        if (isOnGround == true && isStickWall == true)
        //        {
        //            //addD = 3;
        //            rb2D.AddForce(new Vector2(10, 0), ForceMode2D.Impulse);
        //        }
        //        else if (isOnGround == true && isStickWall == false)
        //        {
        //            //addD = 3;
        //            rb2D.AddForce(new Vector2(10, 0), ForceMode2D.Impulse);
        //        }
        //        else if (isOnGround == false && isStickWall == true)
        //        {
        //            //addD = 0;
        //        }
        //        else if (isOnGround == false && isStickWall == false)
        //        {
        //            //addD = 3;
        //            rb2D.AddForce(new Vector2(10, 0), ForceMode2D.Impulse);
        //        }
        //        isPressD = true;
        //    }
        //    else
        //    {
        //        addD = 0;
        //        isPressD = false;
        //    }
        //    if (Input.GetKeyDown(KeyCode.LeftShift))
        //    {
        //        if(dashCooltime<0)
        //        {
        //            isDash = true;
        //            StopCoroutine(ShiftCoroutine());
        //            StartCoroutine(ShiftCoroutine());

        //            dashCooltime = 4;


        //        }

        //    }
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        if(isPressS==false)
        //        {
        //            if (doubleJumpInt > 0&&(isOnGround == false&& isOnAirGround == false))
        //            {
        //                //rigidbody2D.AddForce(new Vector2(0, 300 * mass));
        //                rb2D.velocity = new Vector2(velX, 10);
        //                //AirGround.instance.tilemapCollider2D.isTrigger = true;
        //                AirGround.instance.compositeCollider2D.isTrigger = true;
        //                doubleJumpInt--;
        //            }
        //            if (isOnGround==true||isOnAirGround==true)
        //            {
        //                rb2D.velocity = new Vector2(velX, 10);
        //                //rigidbody2D.AddForce(new Vector2(0, 600*mass));
        //                //AirGround.instance.tilemapCollider2D.isTrigger = true;
        //                AirGround.instance.compositeCollider2D.isTrigger = true;
        //                doubleJumpInt--;

        //            }

        //        }
        //        else if(isPressS==true)
        //        {

        //            //AirGround.instance.tilemapCollider2D.isTrigger = true;
        //            AirGround.instance.compositeCollider2D.isTrigger = true;
        //        }

        //    }
        //}

        //IEnumerator PressSCoroutine()
        //{

        //        yield return new WaitForSecondsRealtime(0.5f);
        //    isPressS = false;

        //}

        //IEnumerator ShiftCoroutine()
        //{
        //    if (isPressA == true)
        //    {
        //        for(int i=0; i<100;i++)
        //        {

        //        rb2D.AddForce(new Vector2(-30, 0),ForceMode2D.Impulse);
        //            yield return new WaitForSecondsRealtime(0.005f);
        //        }
        //    }
        //    else if (isPressD == true)
        //    {
        //        for (int i = 0; i < 100; i++)
        //        {

        //            rb2D.AddForce(new Vector2(30, 0), ForceMode2D.Impulse);
        //            yield return new WaitForSecondsRealtime(0.005f);
        //        }
        //    }
        //    isDash = false;
        //}

        //void GetGravity()
        //{
        //    if(isOnGround==false && isOnAirGround==false && isJump==false)
        //    {
        //        //rigidbody2D.GetVector(new Vector2(playerVelocity.x, -9.8f));
        //        rb2D.AddForce(new Vector2(0, -1), ForceMode2D.Force);
        //    }
        //    else
        //    {
        //        //Debug.Log("!@#!~@#");
        //        //rigidbody2D.GetVector(new Vector2(playerVelocity.x, 0));
        //    }
        //}

        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if (collision.gameObject.layer == 6)
        //    {
        //        isOnGround = true;
        //    }
        //    if (collision.gameObject.layer == 7)
        //    {
        //        isStickWall = true;
        //    }
        //    if (collision.gameObject.layer == 8)
        //    {
        //        isOnAirGround = true;
        //    }
        //}

        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    if (collision.gameObject.layer == 6)
        //    {
        //        isOnGround = false;
        //    }
        //    if (collision.gameObject.layer == 7)
        //    {
        //        isStickWall = false;
        //    }
        //    if (collision.gameObject.layer == 8)
        //    {
        //        if(isPressS==false)
        //        {
        //            isOnAirGround = false;
        //            //AirGround.instance.tilemapCollider2D.isTrigger = false;
        //        }
        //        else
        //        {
        //            isOnAirGround = false;
        //        }
        //    }
        //}


        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //    if(collision.gameObject.layer==6)
        //    {
        //        doubleJumpInt = 2;
        //        isOnGround = true;
        //    }
        //    if (collision.gameObject.layer == 7)
        //    {
        //        isStickWall = true;
        //    }
        //    if (collision.gameObject.layer == 8)
        //    {
        //        doubleJumpInt = 2;
        //        isOnAirGround = true;
        //    }
        //}

        //private void OnCollisionExit2D(Collision2D collision)
        //{
        //    if (collision.gameObject.layer == 6)
        //    {
        //        isOnGround = false;
        //    }
        //    if (collision.gameObject.layer == 7)
        //    {
        //        isStickWall = false;
        //    }
        //    if (collision.gameObject.layer == 8)
        //    {
        //        isOnAirGround = false;
        //    }
        //}

    }
}
