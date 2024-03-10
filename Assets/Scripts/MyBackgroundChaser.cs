using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class MyBackgroundChaser : MonoBehaviour
    {
        private Transform target;

        public float speed;
        private float duration;
        private Sequence sequence;

        private void Start()
        {
            target = Camera.main.transform;
            duration = 1 / speed;
            Vector2 targetPos = target.localPosition;
            transform.localPosition = targetPos;
        }

        private void LateUpdate()
        {
            transform.localPosition = new Vector3(
                Mathf.Lerp(transform.localPosition.x, target.localPosition.x, 0.5f), 
                target.localPosition.y, 
                transform.localPosition.z);




            //float distance = (currentPos - targetPos).sqrMagnitude;
            //if (distance < 0.01f)
            //{
            //    return;
            //}
            //if (sequence.IsActive())
            //{
            //    sequence.Kill();
            //}
            //sequence = DOTween.Sequence();
            //sequence.
            //    Append(transform.DOLocalMoveX(targetPos.x, duration)).
            //    Join(transform.DOLocalMoveY(targetPos.y, duration)).
            //    SetEase(Ease.OutQuad).
            //    OnKill(() => sequence = null).
            //    Play();
        }
    }
}
