using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Tile_Jump : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Enemy>(out var enemy)) {
                enemy.Jump();
            }
        }
    }
}
