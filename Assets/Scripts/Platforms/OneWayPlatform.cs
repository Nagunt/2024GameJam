using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GameJam
{
    public class OneWayPlatform : MonoBehaviour
    {
        PlatformEffector2D platformEffector2D;
        bool isContact = false;
        void OnPlatformDrop()
        {
            Debug.Log("드롭 체크");
            if (isContact) {
                platformEffector2D.rotationalOffset = 180;
                isContact = false;
            }
        }

        private void Awake()
        {
            platformEffector2D = GetComponent<PlatformEffector2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player")) {
                isContact = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            isContact = false;
            platformEffector2D.rotationalOffset = 0;
        }

        private void OnEnable()
        {
            MyEventSystem.Instance.Register(EventType.PlatformDrop, OnPlatformDrop);
        }

        private void OnDisable()
        {
            MyEventSystem.Instance.UnRegister(EventType.PlatformDrop, OnPlatformDrop);
        }
    }
}
