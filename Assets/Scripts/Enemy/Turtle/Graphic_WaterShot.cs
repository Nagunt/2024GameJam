using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Graphic_WaterShot : MonoBehaviour
    {
        Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Shot(Vector2 direction)
        {
            animator.SetTrigger("Shot");
            transform.localRotation = Quaternion.Euler(0, 0, 
                direction.y > 0 ? -Mathf.PingPong(Vector2.Angle(Vector2.left, direction), 180) : Vector2.Angle(Vector2.left, direction));
        }

    }
}
