using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameJam
{
    public class UI_Effect : MonoBehaviour
    {
        private enum Quadrant
        {
            One,
            Two,
            Three,
            Four
        }

        public Canvas canvas;
        public RectTransform screenRect;
        public RectTransform effectRect;
        public Image effectImage;

        private Tweener tweener;

        private void FadeIn(float time, Action onComplete)
        {
            if (canvas.enabled == false)
                canvas.enabled = true;
            if (tweener.IsActive())
                tweener.Kill();
            Debug.Log("FadeIn");
            effectRect.sizeDelta = Vector2.zero;
            effectRect.anchoredPosition = Vector2.zero;
            effectImage.color = Color.black;
            tweener = effectImage.
                DOColor(Color.clear, time).
                OnComplete(() => onComplete?.Invoke()).
                OnKill(() => tweener.Kill()).
                Play();
        }
        private void FadeOut(float time, Action onComplete)
        {
            if (canvas.enabled == false)
                canvas.enabled = true;
            if (tweener.IsActive())
                tweener.Kill();
            Debug.Log("FadeOut");
            effectRect.sizeDelta = Vector2.zero;
            effectRect.anchoredPosition = Vector2.zero;
            effectImage.color = Color.clear;
            tweener = effectImage.
                DOColor(Color.black, time).
                OnComplete(() => onComplete?.Invoke()).
                OnKill(() => tweener.Kill()).
                Play();
        }
        private void WhiteFadeOut(float time, Action onComplete)
        {
            if (canvas.enabled == false)
                canvas.enabled = true;
            if (tweener.IsActive())
                tweener.Kill();
            Debug.Log("WhiteFadeOut");
            effectRect.sizeDelta = Vector2.zero;
            effectRect.anchoredPosition = Vector2.zero;
            effectImage.color = Color.clear;
            tweener = effectImage.
                DOColor(Color.white, time).
                OnComplete(() => onComplete?.Invoke()).
                OnKill(() => tweener.Kill()).
                Play();
        }
        private void RoundFadeIn(Vector2 position, float time, Action onComplete)
        {
            if (canvas.enabled == false)
                canvas.enabled = true;
            if (tweener.IsActive())
                tweener.Kill();
            Debug.Log($"RoundFadeIn At {position}");
            Vector2 point = GetFarthestPoint(CheckQuadrant(position));
            float diameter = Mathf.Abs(Vector2.Distance(point, position)) * 2f;
            
            effectRect.sizeDelta = Vector2.zero;
            effectRect.anchoredPosition = position;
            effectImage.color = Color.black;
            tweener = effectRect.
                DOSizeDelta(new Vector2(diameter, diameter), time).
                OnComplete(() => onComplete?.Invoke()).
                OnKill(() => tweener = null).
                Play();
        }
        private void RoundFadeOut(Vector2 position, float time, Action onComplete)
        {
            if (canvas.enabled == false)
                canvas.enabled = true;
            if (tweener.IsActive())
                tweener.Kill();
            Debug.Log($"RoundFadeOut At {position}");
            Vector2 point = GetFarthestPoint(CheckQuadrant(position));
            float diameter = Mathf.Abs(Vector2.Distance(point, position)) * 2f;
            effectRect.sizeDelta = new Vector2(diameter, diameter);
            effectRect.anchoredPosition = position;
            effectImage.color = Color.black;
            tweener = effectRect.
                DOSizeDelta(Vector2.zero, time).
                OnComplete(() => onComplete?.Invoke()).
                OnKill(() => tweener = null).
                Play();
        }

        private Quadrant CheckQuadrant(Vector2 position)
        {
            Debug.Log($"Screen : {Screen.width} {Screen.height}");
            float halfX = 960;
            float halfY = 540;
            if(position.x < halfX)
            {
                if (position.y < halfY)
                    return Quadrant.Three;
                else 
                    return Quadrant.Two;
            }
            else
            {
                if (position.y < halfY)
                    return Quadrant.Four;
                else
                    return Quadrant.One;
            }
        }

        private Vector2 GetFarthestPoint(Quadrant quadrant)
        {
            return quadrant switch
            {
                Quadrant.One => Vector2.zero,
                Quadrant.Two => new Vector2(1920, 0),
                Quadrant.Three => new Vector2(1920, 1080),
                Quadrant.Four => new Vector2(0, 1080),
                _ => Vector2.zero,
            };
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log(GetComponent<RectTransform>().sizeDelta);
        }

        private void OnEnable()
        {
            Debug.Log("Effect Start Register");
            MyEventSystem.Instance.Register<float, Action>(EventType.FadeIn, FadeIn);
            MyEventSystem.Instance.Register<float, Action>(EventType.FadeOut, FadeOut);
            MyEventSystem.Instance.Register<float, Action>(EventType.WhiteFadeOut, WhiteFadeOut);
            MyEventSystem.Instance.Register<Vector2, float, Action>(EventType.RoundFadeIn, RoundFadeIn);
            MyEventSystem.Instance.Register<Vector2, float, Action>(EventType.RoundFadeOut, RoundFadeOut);
        }

        private void OnDisable()
        {
            Debug.Log("Effect Stop Register");
            MyEventSystem.Instance.UnRegister<float, Action>(EventType.FadeIn, FadeIn);
            MyEventSystem.Instance.UnRegister<float, Action>(EventType.FadeOut, FadeOut);
            MyEventSystem.Instance.UnRegister<float, Action>(EventType.WhiteFadeOut, WhiteFadeOut);
            MyEventSystem.Instance.UnRegister<Vector2, float, Action>(EventType.RoundFadeIn, RoundFadeIn);
            MyEventSystem.Instance.UnRegister<Vector2, float, Action>(EventType.RoundFadeOut, RoundFadeOut);
        }

    }
}
