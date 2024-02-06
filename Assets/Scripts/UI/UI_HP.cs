using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameJam
{
    public class UI_HP : MonoBehaviour
    {
        public Image image;

        void OnHPChanged(int current, int max)
        {
            image.fillAmount = (float)current / max;
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
