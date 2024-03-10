using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameJam
{
    public class UI_HP : MonoBehaviour
    {
        public Image[] image;

        void OnHPChanged(int current, int max)
        {
            for(int i = 0; i < image.Length; i++)
            {
                image[i].enabled = i <= current - 1;
            }

        }

        private void OnEnable()
        {
            MyEventSystem.Instance.Register<int, int>(EventType.HPChanged, OnHPChanged);
        }

        private void OnDisable()
        {
            MyEventSystem.Instance.UnRegister<int, int>(EventType.HPChanged, OnHPChanged);
        }
    }
}
