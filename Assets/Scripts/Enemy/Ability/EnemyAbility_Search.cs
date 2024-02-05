using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class EnemyAbility_Search : EnemyAbility
    {
        public Collider2D searchArea;
        public ContactFilter2D searchFilter;
        private List<Collider2D> searchResults = new List<Collider2D>();

        private void FixedUpdate()
        {
            if (searchArea) {
                searchResults.Clear();
                if (Physics2D.OverlapCollider(searchArea, searchFilter, searchResults) > 0) {
                    foreach (Collider2D col2D in searchResults) {
                        if (col2D.CompareTag("Player")) {
                            owner.SetTarget(col2D.transform);
                        }
                    }
                }
                else {
                    // searchArea 밖에 플레이어가 나가면 타겟을 초기화한다
                    owner.SetTarget(null);
                }
            }
        }
    }
}
