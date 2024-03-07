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

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(nameof(Routine));
        }

        IEnumerator Routine()
        {
            while(index < stages.Length) {
                current = Instantiate(stages[index]);
                current.Init();
                yield return new WaitUntil(() => _isClear);
                yield return new WaitUntil(() => _isNextStage);
                Destroy(current.gameObject);
                DestroyImmediate(Player.Instance.gameObject);
                yield return null;
                index++;
            }
        }

        private bool _isClear = false;
        private bool _isNextStage = false;
        private void OnStageStart()
        {
            gameObject.GetComponent<AudioSource>().Stop();
            if (index < 3)
            {
            gameObject.GetComponent<AudioSource>().clip = GameJam.MyGameManager.Instance.bGMAudioClips[0];

            }
            else
            {
                gameObject.GetComponent<AudioSource>().clip = GameJam.MyGameManager.Instance.bGMAudioClips[1];
            }
            gameObject.GetComponent<AudioSource>().Play();

            _isClear = false;
            _isNextStage = false;
        }
        private void OnStageClear()
        {
            _isClear = true;
        }

        private void OnStageRestart()
        {
            StopCoroutine(nameof(Routine));
            SceneManager.LoadScene("World");
            StartCoroutine(nameof(Routine));
        }

        private void OnNextStage()
        {
            _isNextStage = true;
        }

        private void OnEnable()
        {
            MyEventSystem.Instance.Register(EventType.StageStart, OnStageStart);
            MyEventSystem.Instance.Register(EventType.StageClear, OnStageClear);
            MyEventSystem.Instance.Register(EventType.StageRestart, OnStageRestart);
            MyEventSystem.Instance.Register(EventType.NextStage, OnNextStage);
        }

        private void OnDisable()
        {
            MyEventSystem.Instance.UnRegister(EventType.StageStart, OnStageStart);
            MyEventSystem.Instance.UnRegister(EventType.StageClear, OnStageClear);
            MyEventSystem.Instance.UnRegister(EventType.StageRestart, OnStageRestart);
            MyEventSystem.Instance.UnRegister(EventType.NextStage, OnNextStage);
        }
    }
}
