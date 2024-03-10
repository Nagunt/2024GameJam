using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameJam
{
    public class UI_Main : MonoBehaviour
    {
        public Button button;
        public Text text;

        private bool isNext = false;
        private Sequence sequence;

        private void Awake()
        {
            button.onClick.AddListener(() => {
                if (isNext) return;
                if (sequence.IsActive())
                {
                    sequence.Kill();
                }
                isNext = true;
                
                MyEventSystem.Instance.Call<float, Action>(EventType.FadeOut, 1f, () =>
                {
                    SceneManager.LoadScene("World");
                });
            });
        }
        private void Start()
        {
            sequence = DOTween.Sequence();

            sequence.
                Append(text.DOFade(0, 1.2f).SetEase(Ease.InQuad)).
                Append(text.DOFade(1, 1.2f).SetEase(Ease.InQuad)).
                SetLoops(-1).
                OnKill(() => sequence = null).
                Play();
        }

        private void Update()
        {
            if (isNext == false && Input.anyKey)
            {
                if (sequence.IsActive())
                {
                    sequence.Kill();
                }
                isNext = true;
                MyEventSystem.Instance.Call<float, Action>(EventType.FadeOut, 1f, () =>
                {
                    SceneManager.LoadScene("World");
                });
            }
        }


    }
}
