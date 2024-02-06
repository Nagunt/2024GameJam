using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class MyGameManager : MonoBehaviour
    {
        public static MyGameManager Instance { get; private set; } = null;

        public Stage[] stages;
        public int index;

        public Player playerPrefab;
        private Stage current;
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
            while(index < stages.Length) {
                current = Instantiate(stages[index], transform);
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
            Debug.Log($"스테이지 {index} 시작");
            _isClear = false;
            _isNextStage = false;
        }
        private void OnStageClear()
        {
            Debug.Log($"스테이지 {index} 클리어");
            _isClear = true;
        }

        private void OnStageRestart()
        {
            StopCoroutine(nameof(Routine));
            Destroy(current.gameObject);
            if (Player.Instance.gameObject != null) {
                DestroyImmediate(Player.Instance.gameObject);
            }
            StartCoroutine(nameof(Routine));
        }

        private void OnNextStage()
        {
            Debug.Log($"다음 스테이지로~");
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
