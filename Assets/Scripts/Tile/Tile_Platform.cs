using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Tile_Platform : MonoBehaviour
    {
        public Rigidbody2D rb2D;
        public Collider2D col2D;

        private bool isIgnore = false;
        private void OnPlatformDrop(Collider2D col, Vector2 pos)
        {
            Debug.Log("PlatformDrop");
            if(pos.y >= rb2D.position.y) {
                Physics2D.IgnoreCollision(col2D, col, false);
            }
            else {
                Physics2D.IgnoreCollision(col2D, col);
            }
            isIgnore = true;
        }

        private void OnEnable()
        {
            MyEventSystem.Instance.Register<Collider2D, Vector2>(EventType.PlatformDrop, OnPlatformDrop);
        }

        private void OnDisable()
        {
            MyEventSystem.Instance.UnRegister<Collider2D, Vector2>(EventType.PlatformDrop, OnPlatformDrop);
        }

        private void FixedUpdate()
        {
            if (isIgnore) {
                isIgnore = false;
                return;
            }
            if (rb2D == null || col2D == null) return;
            if (Player.instance == null) return;
            if (Player.instance.rb2D.position.y >= rb2D.position.y) {
                if (Physics2D.GetIgnoreCollision(col2D, Player.instance.col2D)) {
                    Physics2D.IgnoreCollision(col2D, Player.instance.col2D, false);
                }
            }
            else {
                Physics2D.IgnoreCollision(col2D, Player.instance.col2D);
            }
        }
    }
}
