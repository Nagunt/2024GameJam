using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class EnemyAbility_Patrol : EnemyAbility
    {
        public Transform[] position;
        public int firstIndex = 0;
        private List<Vector2> positions;

        private int index = 0;

        public int Index {
            get {
                return index;
            }
            set {
                index = value;
                if (index < 0) index = 0;
                if (index >= positions.Count) index = 0;
                owner.SetDestination(positions[index]);
            }
        }

        private void Start()
        {
            positions = new List<Vector2>(position.Length);
            for(int i = 0; i < position.Length; i++) {
                positions.Add(position[i].position);
            }
            Index = firstIndex;
        }

        void OnArrive(Enemy enemy, Vector2 pos)
        {
            Debug.Log("Arrived : " + enemy.name + " Position : " + pos);
            if (enemy == owner &&
                positions[Index] == pos) {
                Index++;
            }
        }

        private void OnEnable()
        {
            Debug.Log("Event Register");
            MyEventSystem.Instance.Register<Enemy, Vector2>(EventType.Arrive, OnArrive);
        }

        private void OnDisable()
        {
            Debug.Log("Event UnRegister");
            MyEventSystem.Instance.UnRegister<Enemy, Vector2>(EventType.Arrive, OnArrive);
        }
    }
}