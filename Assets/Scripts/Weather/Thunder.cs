using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Thunder : MonoBehaviour
    {

        public ThunderGraphic thunderGraphic;

        public int damage = 2;
        public float power = 0f;

        private void Awake()
        {
            thunderGraphic = GetComponentInChildren<ThunderGraphic>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out Player Player))
            {
                Player.Hit(damage, transform.position, power);
                Destroy(gameObject);
            }
        }
    }
}
