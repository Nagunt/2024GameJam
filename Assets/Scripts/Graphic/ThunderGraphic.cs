using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    [RequireComponent(typeof(Animator))]
    public class ThunderGraphic : MonoBehaviour
    {
        private Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        public void SetTrigger()
        {
            animator.SetTrigger("ThunderCall");
        }
    }
}

