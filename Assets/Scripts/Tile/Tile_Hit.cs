using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Tile_Hit : MonoBehaviour
    {
        public int damage = 1;
        public float power = 50f;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out Player Player)) {
                Player.Hit(damage, transform.position, power);
            }
        }
    }
}
