using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Turtle : MonoBehaviour
    {
        Graphic_Turtle graphic;
        public Collider2D playerHitBox;
        public Collider2D hitBox;

        public ContactFilter2D contactFilter;
        public List<Collider2D> colliders;

        public Transform firstPos;
        public Transform shotPos;
        Vector2 _moveDir;
        public float moveSpeed = 4f;
        Rigidbody2D rb2D;

        public WaterShot shotPrefab;

        int hp = 3;
        bool isAlive => hp > 0;

        public int damage = 1;
        public float power = 90f;
        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
            graphic = GetComponentInChildren<Graphic_Turtle>();
            colliders = new List<Collider2D>();
        }
        public void Spawn()
        {
            StartCoroutine(Routine());
        }
        private bool _isHit = true;
        private float _hitImmuneTime = 3f;
        // Update is called once per frame
        private WaterShot newShot;
        IEnumerator Routine()
        {
            gameObject.GetComponent<AudioSource>().clip = GameJam.MyGameManager.Instance.BossAudioClips[0];
            gameObject.GetComponent<AudioSource>().Play();
            rb2D.bodyType = RigidbodyType2D.Kinematic;
            bool isEnd = false;
            transform.
                DOMove(firstPos.position, 5f).
                OnComplete(() => isEnd = true).
                Play();
            yield return new WaitUntil(() => isEnd);
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            _isHit = false;
            while (isAlive) {
                newShot = Instantiate(shotPrefab, shotPos);
                graphic.SetCharge();
                yield return new WaitForSeconds(3f);
                if(Player.Instance.gameObject != null) {
                    gameObject.GetComponent<AudioSource>().clip = GameJam.MyGameManager.Instance.BossAudioClips[3];
                    gameObject.GetComponent<AudioSource>().Play();
                    newShot.Shot(Player.Instance.transform.position - newShot.transform.position);
                }
                newShot = null;
                graphic.SetShoot();
                yield return new WaitForSeconds(4f);
                _moveDir = new Vector2(-1, 0);
                gameObject.GetComponent<AudioSource>().clip = GameJam.MyGameManager.Instance.BossAudioClips[1];
                gameObject.GetComponent<AudioSource>().Play();
                graphic.SetSpin(true);
                yield return new WaitForSeconds(3f);
                _moveDir = new Vector2(1, 0);
                yield return new WaitForSeconds(3f);
                _moveDir = new Vector2(-1, 0);
                yield return new WaitForSeconds(3f);
                _moveDir = new Vector2(1, 0);
                yield return new WaitForSeconds(3f);
                _moveDir = new Vector2(-1, 0);
                yield return new WaitForSeconds(3f);
                _moveDir = new Vector2(0, 0);
                graphic.SetSpin(false);
                yield return new WaitForSeconds(5f);
            }
        }

        private void Update()
        {
            if(!isAlive) return;
            if (Player.Instance.gameObject != null) {
                if(Player.Instance.transform.position.x > transform.position.x) {
                    graphic.transform.localScale = new Vector3(1, 1, 1);
                }
                else {
                    graphic.transform.localScale = new Vector3(-1, 1, 1);
                }
            }
        }


        private void FixedUpdate()
        {
            if(_moveDir.x != 0) {
                rb2D.velocity = new Vector2(_moveDir.x * moveSpeed, rb2D.velocity.y);
            }
            else {
                rb2D.velocity = new Vector2(0, rb2D.velocity.y);
            }
            if (_isHit) return;
            if(Physics2D.OverlapCollider(playerHitBox, contactFilter, colliders) > 0) {
                foreach(Collider2D collider in colliders) {
                    if (collider.CompareTag("Player")) {
                        hp -= 1;
                        if(isAlive) {
                            collider.attachedRigidbody.AddForce(Vector2.up * 20f, ForceMode2D.Impulse);
                            StartCoroutine(nameof(HitRoutine));
                            return;
                        }
                        else {
                            _isHit = true;
                            gameObject.GetComponent<AudioSource>().clip = GameJam.MyGameManager.Instance.BossAudioClips[4];
                            gameObject.GetComponent<AudioSource>().Play();
                            graphic.SetDead();
                            if(newShot != null) Destroy(newShot.gameObject);
                            StopAllCoroutines();
                            transform.DOMove(transform.position - new Vector3(0, 10, 0), 5f).Play();
                            MyEventSystem.Instance.Call(EventType.GameClear);
                            return;
                        }
                    }
                }
            }
            if (Physics2D.OverlapCollider(hitBox, contactFilter, colliders) > 0) {
                foreach (Collider2D collider in colliders) {
                    if (collider.CompareTag("Player")) {
                        Player.Instance.Hit(damage, transform.position, power);
                    }
                }
            }
        }

        private IEnumerator HitRoutine()
        {
            _isHit = true;
            graphic.SetHitEffect(true);
            float startTime = Time.time;
            while (Time.time - startTime <= _hitImmuneTime) {
                yield return null;
            }
            graphic.SetHitEffect(false);
            _isHit = false;
        }
    }
}
