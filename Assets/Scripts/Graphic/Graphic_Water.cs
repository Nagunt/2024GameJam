using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Graphic_Water : MonoBehaviour
    {
        private Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Play()
        {
            animator.SetTrigger("Enter");
        }
        public void Stop()
        {
            animator.SetTrigger("Exit");
        }
    }
}
