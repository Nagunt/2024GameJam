using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Graphic_Turtle : MonoBehaviour
    {
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        private bool _isEffect;
        private Sequence _effectSequence;
        public void SetHitEffect(bool state)
        {
            if (_isEffect != state) {
                _isEffect = state;
                if (_isEffect) {
                    _effectSequence = DOTween.Sequence(this);
                    _effectSequence.
                        OnStart(() => {
                            spriteRenderer.color = Color.white;
                        }).
                        Append(spriteRenderer.DOColor(Color.red, 0.1f)).
                        Append(spriteRenderer.DOColor(Color.white, 0.1f)).
                        SetLoops(-1).
                        OnKill(() => {
                            spriteRenderer.color = Color.white;
                            _effectSequence = null;
                        }).
                        Play();
                }
                else {
                    _effectSequence.Kill();
                }
            }
        }

        public void SetDead()
        {
            animator.SetTrigger("Dead");
        }
        public void SetFilpX(bool state)
        {
            spriteRenderer.flipX = state;
        }
        public void SetCharge()
        {
            animator.SetTrigger("Charge");
        }
        public void SetShoot()
        {
            animator.SetTrigger("Shoot");
        }

        public void SetSpin(bool state)
        {
            animator.SetBool("IsSpin", state);
        }
    }
}
