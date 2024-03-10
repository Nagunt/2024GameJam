using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Tile_Start : MonoBehaviour
    {
        private void OnStageStart()
        {
            Player.Instance.transform.position = transform.position;
            Player.Instance.IsFacingRight = true;
        }

        private void OnEnable()
        {
            MyEventSystem.Instance.Register(EventType.StageStart, OnStageStart);
        }

        private void OnDisable()
        {
            MyEventSystem.Instance.UnRegister(EventType.StageStart, OnStageStart);
        }
    }
}
