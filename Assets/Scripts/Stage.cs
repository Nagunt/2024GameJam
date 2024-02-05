using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace GameJam
{
    public class Stage : MonoBehaviour
    {
        public CinemachineVirtualCamera virtualCamera;
        public void Init()
        {
            MyEventSystem.Instance.Call(EventType.StageStart);
            virtualCamera.Follow = Player.Instance.transform;
        }
    }
}
