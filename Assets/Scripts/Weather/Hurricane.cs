using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameJam
{
    public class Hurricane : MonoBehaviour
    {
        private Rigidbody2D rb2D;

        public bool isGrounded;
        private int groundMask;
        private float groundDistance = 0.05f;

        public int damage = 0;
        public float power = 50f;

        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
            groundMask = LayerMask.GetMask("Ground");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.TryGetComponent<Player>(out Player Player))
            {
                Player.Hit(damage, transform.position, power);
                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);
            if (isGrounded == false)
            {
                Destroy(gameObject);
            }
            rb2D.velocity += new Vector2(-7f * 0.1f, 0);
        }
    }
}
