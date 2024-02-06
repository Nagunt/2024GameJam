using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace GameJam
{
    public class Graphic_Player : MonoBehaviour
    {
        private Animator animator;
        private Player owner;
        private SpriteRenderer spriteRenderer;

        public bool startedJumping { private get; set; }
        public bool justLanded { private get; set; }
        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public void SetOwner(Player owner)
        {
            this.owner = owner;
        }

        private bool _isEffect;
        private Sequence _effectSequence;
        public void SetHitEffect(bool state)
        {
            if(_isEffect != state) {
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

        private void LateUpdate()
        {
            CheckAnimationState();
        }

        private void CheckAnimationState()
        {
            if (startedJumping) {
                animator.SetTrigger("Jump");
                startedJumping = false;
                return;
            }
            if (justLanded) {
                animator.SetTrigger("Land");
                justLanded = false;
                return;
            }
            animator.SetFloat("speed", Mathf.Abs(owner.rb2D.velocity.x));
        }
    }
}
