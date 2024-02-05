using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class EnemyAbility : MonoBehaviour
    {
        protected Enemy owner;

        private void Awake()
        {
            owner = GetComponent<Enemy>();
        }
    }
}