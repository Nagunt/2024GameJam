using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameJam
{
    public class UI_GameOver : MonoBehaviour
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
        public void OnGameOver()
        {
            SetState(true);
        }
        private void OnClick_Next()
        {
            MyEventSystem.Instance.Call<float, Action>(EventType.FadeOut, 1f, () =>
            {
                SetState(false);
                MyEventSystem.Instance.Call(EventType.StageRestart);
            });
        }
        private void Awake()
        {
            SetState(false);
            button_Next.onClick.AddListener(OnClick_Next);
        }
        private void OnEnable()
        {
            MyEventSystem.Instance.Register(EventType.GameOver, OnGameOver);
        }

        private void OnDisable()
        {
            MyEventSystem.Instance.UnRegister(EventType.GameOver, OnGameOver);
        }
    }
}
