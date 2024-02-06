using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{


    public class WaterShot : MonoBehaviour
    {
        public int damage = 1;
        public float power = 50f;
        public float speed = 2f;

        public Collider2D col2D;
        public ContactFilter2D contactFilter;
        public List<Collider2D> colliders;
        Graphic_WaterShot graphic;
        // Start is called before the first frame update
        private void Awake()
        {
            colliders = new List<Collider2D>();
            graphic = GetComponentInChildren<Graphic_WaterShot>();
        }

        Vector2 _direction;
        public void Shot(Vector2 direction)
        {
            _isShot = true;
            _direction = direction;
            graphic.Shot(direction);
        }

        bool _isShot = false;
        private void FixedUpdate()
        {
            if(_isShot) {
                transform.position += (Vector3)_direction * speed * Time.fixedDeltaTime;

                if(Physics2D.OverlapCollider(col2D, contactFilter, colliders) > 0) {
                    foreach (Collider2D col in colliders) {
                        if (col.TryGetComponent<Player>(out Player player)) {
                            player.Hit(damage, transform.position, power);
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }
    }
}
