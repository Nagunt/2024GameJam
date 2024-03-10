using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJam
{
    public class UI_GameClear : MonoBehaviour
    {
        public Canvas canvas;
        public void OnClick()
        {
            canvas.sortingOrder = 0;
            MyEventSystem.Instance.Call<float, Action>(EventType.FadeOut, 1f, () =>
            {
                Application.Quit();
            });
        }
    }
}
