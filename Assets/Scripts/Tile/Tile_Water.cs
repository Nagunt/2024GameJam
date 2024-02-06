using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameJam
{
    public class Tile_Water : MonoBehaviour
    {
        public int damage = 1;
        public float power = 50f;
        public BoxCollider2D col2D;
        public Graphic_Water graphic;

        private void Start()
        {
            StartCoroutine(nameof(Routine));
        }

        IEnumerator Routine()
        {
            while (true) {
                yield return new WaitForSeconds(3f);
                graphic.Play();
                DOTween.To(() => col2D.size, x => col2D.size = x, new Vector2(1, 7.5f), 0.5f);
                yield return new WaitForSeconds(6f);
                graphic.Stop();
                DOTween.To(() => col2D.size, x => col2D.size = x, new Vector2(1, 0.1f), 0.5f);
                yield return new WaitForSeconds(3f);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out Player Player)) {
                Player.Hit(damage, transform.position, power);
            }
        }
    }
}
