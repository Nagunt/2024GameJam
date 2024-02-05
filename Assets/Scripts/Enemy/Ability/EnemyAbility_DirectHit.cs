using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class EnemyAbility_DirectHit : MonoBehaviour
    {

        public int damage;
        public Collider2D hitBox;
        public ContactFilter2D hitFilter;
        private List<Collider2D> hitResults = new List<Collider2D>();

        private void FixedUpdate()
        {
            if (hitBox) {
                hitResults.Clear();
                if (Physics2D.OverlapCollider(hitBox, hitFilter, hitResults) > 0) {
                    foreach (Collider2D col2D in hitResults) {
                        if (col2D.CompareTag("Player")) {
                            Debug.Log("Damage : " + damage);
                        }
                    }
                }
            }
        }
    }
}
