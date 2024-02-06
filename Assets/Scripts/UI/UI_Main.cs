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


        private void Awake()
        {
            button.onClick.AddListener(() => {
                SceneManager.LoadScene("World");
            });
        }
    }
}
