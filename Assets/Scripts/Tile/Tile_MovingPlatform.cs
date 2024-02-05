using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GameJam
{
    public enum MovingType
    {
        NextTo,
        Circle
    }

    public class Tile_MovingPlatform : MonoBehaviour
    {
        public Rigidbody2D rb2D;
        public Collider2D col2D;

        public Transform[] position;
        public int firstIndex = 0;
        private List<Vector2> positions;
        private int index = 0;

        public float movingSpeed = 2f;
        public MovingType movingType = MovingType.NextTo;

        public int Index {
            get {
                return index;
            }
            set {
                index = value;
                if (index < 0) index = 0;
                if (index >= positions.Count) index = positions.Count - 1;
                SetDestination(positions[index]);
            }
        }
        int nextValue;
        Vector2 destination;
        private void SetDestination(Vector2 position)
        {
            destination = position;
        }

        private bool isIgnore = false;
        private void OnPlatformDrop(Collider2D col, Vector2 pos)
        {
            if (pos.y >= rb2D.position.y) {
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

        private void Start()
        {
            positions = new List<Vector2>(position.Length);
            for (int i = 0; i < position.Length; i++) {
                positions.Add(position[i].position);
            }
            nextValue = 1;
            Index = firstIndex;
        }

        private void FixedUpdate()
        {
            if (Vector2.SqrMagnitude(rb2D.position - destination) > 0.01f) {
                Vector2 direction = destination - rb2D.position;
                rb2D.velocity = direction.normalized * movingSpeed;
            }
            else {
                if (movingType == MovingType.NextTo) {
                    if (Index == 1 && nextValue == -1) {
                        nextValue = 1;
                        Index = 0;
                    }
                    else if (Index == positions.Count - 2 && nextValue == 1) {
                        nextValue = -1;
                        Index = positions.Count - 1;
                    }
                    else Index += nextValue;
                }
                else if(movingType == MovingType.Circle) {
                    if (Index == positions.Count - 1) {
                        Index = 0;
                    }
                    else Index += 1;
                }
            }


            if (isIgnore) {
                isIgnore = false;
                return;
            }
            if (rb2D == null || col2D == null) return;
            if (Player.Instance == null) return;
            if (Player.Instance.rb2D.position.y >= rb2D.position.y) {
                if (Physics2D.GetIgnoreCollision(col2D, Player.Instance.col2D)) {
                    Physics2D.IgnoreCollision(col2D, Player.Instance.col2D, false);
                }
            }
            else {
                Physics2D.IgnoreCollision(col2D, Player.Instance.col2D);
            }
        }

    }
}
