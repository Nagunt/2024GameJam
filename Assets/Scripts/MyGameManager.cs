using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameJam
{
    public class MyGameManager : MonoBehaviour
    {
        public static MyGameManager Instance { get; private set; } = null;

        public Stage[] stages;
        public int index;

        public Player playerPrefab;
        private Stage current;

        public AudioClip[] playerAudioClips;
        public AudioClip[] bGMAudioClips;
        public AudioClip[] BossAudioClips;
        public AudioSource audioSource;

        private void Awake()
        {
            Instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(nameof(Routine));
        }

        IEnumerator Routine()
        {
            Instantiate(playerPrefab);
            audioSource.clip = bGMAudioClips[0];
            while (index < stages.Length)
            {
                current = Instantiate(stages[index]);
                current.Init();

                _isClear = false;

                if (index == stages.Length - 1)
                {
                    audioSource.clip = bGMAudioClips[1];
                }
                audioSource.Play();

                MyEventSystem.Instance.Call(EventType.StageStart);
                yield return null;
                MyEventSystem.Instance.Call<Vector2, float, Action>(
                    EventType.RoundFadeIn,
                    Camera.main.WorldToScreenPoint(Player.Instance.TargetPoint),
                    1f,
                    () => Player.Instance.SetPhysics(true));
                yield return new WaitUntil(() => _isClear);
                Player.Instance.SetPhysics(false);
                if (index == stages.Length - 1)
                {
                    break;
                }
                audioSource.Pause();
                bool isEnd = false;
                MyEventSystem.Instance.Call<Vector2, float, Action>(
                    EventType.RoundFadeOut,
                    Camera.main.WorldToScreenPoint(Player.Instance.TargetPoint),
                    1f,
                    () => isEnd = true);
                yield return new WaitUntil(() => isEnd);
                Destroy(current.gameObject);
                yield return null;
                index++;
            }
            MyEventSystem.Instance.Call<float, Action>(
                    EventType.WhiteFadeOut,
                    3f,
                    () => SceneManager.LoadScene("GameClear"));
        }

        private bool _isClear = false;
        private void OnStageClear()
        {
            _isClear = true;
        }

        private void OnStageRestart()
        {
            StopCoroutine(nameof(Routine));
            SceneManager.LoadScene("World");
        }

        private void OnEnable()
        {
            MyEventSystem.Instance.Register(EventType.StageClear, OnStageClear);
            MyEventSystem.Instance.Register(EventType.StageRestart, OnStageRestart);
        }

        private void OnDisable()
        {
            MyEventSystem.Instance.UnRegister(EventType.StageClear, OnStageClear);
            MyEventSystem.Instance.UnRegister(EventType.StageRestart, OnStageRestart);
        }
    }
}
