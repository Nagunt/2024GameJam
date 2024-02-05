using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class FallingPlatform : MonoBehaviour
    {
        public float fallingDelay = 1f;
        public float destroyDelay = 2f;

        private Rigidbody2D rb2D;
        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player")) {
                StartCoroutine(FallRoutine());
            }
        }

        IEnumerator FallRoutine()
        {
            yield return new WaitForSeconds(fallingDelay);
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            Destroy(gameObject, destroyDelay);
        }

    }
}
