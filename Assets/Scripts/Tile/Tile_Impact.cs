using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Tile_Impact : MonoBehaviour
    {
        public float power = 100f;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out Player Player)) {
                Vector2 direction = collision.transform.position - transform.position;
                Player.rb2D.AddForce(direction.normalized * power, ForceMode2D.Impulse);
            }
        }
    }
}
