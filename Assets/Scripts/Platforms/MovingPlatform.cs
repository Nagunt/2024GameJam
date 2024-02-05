using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public enum MoveingType
    {
        Round,
        PingPong,
    }
    public class MovingPlatform : MonoBehaviour
    {
        public float speed = 2f;
        public int startingPoint;
        public Transform[] points;
        
        public MoveingType moveingType;
        private Vector2 destination;
        private int index;
        // Start is called before the first frame update
        void Start()
        {
            destination = points[startingPoint].position;
        }

        // Update is called once per frame
        void Update()
        {
            if(Vector2.Distance(transform.position, points[index].position) < 0.01f) {
                index++;
                if (moveingType == MoveingType.Round) {
                    if (index == points.Length) {
                        index = 0;
                    }
                    destination = points[index].position;
                }
                else if(moveingType == MoveingType.PingPong) {
                    destination = points[(int)Mathf.PingPong(index, points.Length)].position;
                }

            }
            transform.position = Vector2.MoveTowards(
                transform.position,
                destination,
                speed * Time.deltaTime);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player")) {
                if(transform.position.y < collision.transform.position.y) {
                    collision.transform.SetParent(transform);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player")) {
                collision.transform.SetParent(null);
            }
        }
    }
}
