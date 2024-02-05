using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class EnemyAbility_Patrol : EnemyAbility
    {
        public Transform[] position;
        private List<Vector2> positions;

        private int index = 0;

        private void Start()
        {
            positions = new List<Vector2>(position.Length);
            for(int i = 0; i < position.Length; i++) {
                positions[i] = position[i].position;
            }
        }

        private void Update()
        {
            
        }

        void OnArrive(Enemy enemy, Vector2 pos)
        {
            if (enemy == owner) {

            }
        }

        private void OnEnable()
        {
            MyEventSystem.Instance.Register<Enemy, Vector2>(EventType.Arrive, OnArrive);
        }

        private void OnDisable()
        {
            MyEventSystem.Instance.UnRegister<Enemy, Vector2>(EventType.Arrive, OnArrive);
        }
    }
}