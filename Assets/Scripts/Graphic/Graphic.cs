using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    [RequireComponent(typeof(Animator))]
    public class Graphic : MonoBehaviour
    {
        private Animator animator;
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        public void SetMoveDirection(float moveDirection)
        {
            animator.SetFloat("MoveDirection", moveDirection);
        }
    }
}
