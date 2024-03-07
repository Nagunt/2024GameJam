using DG.Tweening;
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

        private Sequence sequence;

        private void Awake()
        {
            button.onClick.AddListener(() => {
                if (sequence.IsActive())
                {
                    sequence.Kill();
                }
                SceneManager.LoadScene("World");
            });
        }
        private void Start()
        {
            sequence = DOTween.Sequence();

            sequence.
                Append(text.DOFade(0, 1.2f)).
                Append(text.DOFade(1, 1.2f)).
                SetLoops(-1).
                SetEase(Ease.Linear).
                OnKill(() => sequence = null).
                Play();
        }

        private void Update()
        {
            if (Input.anyKey)
            {
                if (sequence.IsActive())
                {
                    sequence.Kill();
                }
                SceneManager.LoadScene("World");
            }
        }


    }
}
