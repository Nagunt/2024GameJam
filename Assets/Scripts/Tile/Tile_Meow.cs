using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameJam
{
    public class Tile_Meow : MonoBehaviour
    {
        public Transform[] position;
        public int firstIndex = 0;
        private List<Vector2> positions;
        Vector2 destination;
        public float moveSpeed = 2f;
        public int damage = 1;
        public float power = 50f;

        private int index = 0;

        public int Index {
            get {
                return index;
            }
            set {
                index = value;
                if (index < 0) index = 0;
                if (index >= positions.Count) index = 0;
                destination = positions[index];
            }
        }

        bool IsArrived(Vector2 position)
        {
            return ((Vector2)transform.position - position).sqrMagnitude < 0.05f;
        }

        private void Start()
        {
            positions = new List<Vector2>(position.Length);
            for (int i = 0; i < position.Length; i++) {
                positions.Add(position[i].position);
            }
            Index = firstIndex;
        }

        private void Update()
        {
            if (IsArrived(positions[Index])) {
                Index++;
            }
            Vector2 direction = destination - (Vector2)transform.position;
            transform.position += moveSpeed * Time.deltaTime * (Vector3)direction.normalized;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out Player Player)) {
                Player.Hit(damage, transform.position, power);
            }
        }
    }
}
