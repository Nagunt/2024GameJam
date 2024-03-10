using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJam
{
    public class Tile_GameClear : MonoBehaviour
    {
        public Transform moveTo;
        void OnGameClear()
        {
            transform.position = moveTo.position;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out Player Player)) {
                MyEventSystem.Instance.Call(EventType.StageClear);
            }
        }

        private void OnEnable()
        {
            MyEventSystem.Instance.Register(EventType.GameClear, OnGameClear);
        }

        private void OnDisable()
        {
            MyEventSystem.Instance.UnRegister(EventType.GameClear, OnGameClear);
        }
    }
}
