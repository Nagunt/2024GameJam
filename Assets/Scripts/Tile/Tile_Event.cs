using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Tile_Event : MonoBehaviour
    {
        bool isExecuted = false;
        public Turtle turtle;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(isExecuted) return;
            if (collision.TryGetComponent<Player>(out Player Player))
            {
                isExecuted = true;
                turtle.Spawn();
            }
        }
    }
}
