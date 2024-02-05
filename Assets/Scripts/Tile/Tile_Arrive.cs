using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Tile_Arrive : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log(collision.name);
            if (collision.TryGetComponent<Enemy>(out Enemy enemy)) {
                MyEventSystem.Instance.Call<Enemy, Vector2>(EventType.Arrive, enemy, transform.position);
            }
        }
    }
}