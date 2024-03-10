using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

namespace GameJam
{
    public class Stage : MonoBehaviour
    {
        public CinemachineVirtualCamera virtualCamera;
        public void Init()
        {
            virtualCamera.Follow = Player.Instance.transform;
        }
    }
}
