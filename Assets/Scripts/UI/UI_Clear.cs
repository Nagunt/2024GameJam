using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameJam
{
    public class UI_Clear : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public Button button_Next;
       
        public void SetState(bool state)
        {
            if (state) {
                canvasGroup.alpha = 1;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            else {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
        }
        private void OnStageClear()
        {
            SetState(true);
        }
        private void OnClick_Next()
        {
            MyEventSystem.Instance.Call(EventType.NextStage);
            SetState(false);
        }

        private void Awake()
        {
            SetState(false);
            button_Next.onClick.AddListener(OnClick_Next);
        }
        private void OnEnable()
        {
            MyEventSystem.Instance.Register(EventType.StageClear, OnStageClear);
        }

        private void OnDisable()
        {
            MyEventSystem.Instance.UnRegister(EventType.StageClear, OnStageClear);
        }
    }
}
