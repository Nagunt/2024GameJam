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

        private void Update()
        {
            if (owner.IsArrived(positions[Index])) {
                Index++;
            }
        }
    }
}