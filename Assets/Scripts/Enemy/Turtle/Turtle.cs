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

        int hp = 5;
        bool isAlive => hp > 0;

        public int damage = 2;
        public float power = 90f;
        private void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
            graphic = GetComponentInChildren<Graphic_Turtle>();
            colliders = new List<Collider2D>();
        }
        // Start is called before the first frame update
        void Start()
        {
            gameObject.GetComponent<AudioSource>().clip = GameJam.MyGameManager.Instance.BossAudioClips[0];
            gameObject.GetComponent<AudioSource>().Play();
            StartCoroutine(nameof(Routine));
        }


        private bool _isHit = true;
        private float _hitImmuneTime = 3f;
        // Update is called once per frame
        private WaterShot newShot;
        IEnumerator Routine()
        {
            rb2D.bodyType = RigidbodyType2D.Kinematic;
            yield return transform.DOMove(firstPos.position, 5f);
            yield return new WaitForSeconds(3f);
            rb2D.bodyType = RigidbodyType2D.Dynamic;
            _isHit = false;
            while (isAlive) {
                newShot = Instantiate(shotPrefab, shotPos);
                graphic.SetFilpX(true);
                graphic.SetCharge();
                yield return new WaitForSeconds(3f);
                if(Player.Instance.gameObject != null) {
                    gameObject.GetComponent<AudioSource>().clip = GameJam.MyGameManager.Instance.BossAudioClips[3];
                    gameObject.GetComponent<AudioSource>().Play();
                    newShot.Shot(Player.Instance.transform.position - newShot.transform.position);
                }
                newShot = null;
                graphic.SetShoot();
                yield return new WaitForSeconds(1f);
                graphic.SetFilpX(false);
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
            if (Player.Instance.gameObject != null) {
                if(Player.Instance.transform.position.x > transform.position.x) {
                    graphic.transform.localScale = new Vector3(-1, 1, 1);
                }
                else {
                    graphic.transform.localScale = new Vector3(1, 1, 1);
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
                            gameObject.GetComponent<AudioSource>().clip = GameJam.MyGameManager.Instance.BossAudioClips[4];
                            gameObject.GetComponent<AudioSource>().Play();
                            graphic.SetDead();
                            if(newShot != null) Destroy(newShot.gameObject);
                            StopAllCoroutines();
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
            // Hit �ִϸ��̼� ����
            graphic.SetHitEffect(true);
            float startTime = Time.time;
            // Hit �ִϸ��̼� ���� ����
            while (Time.time - startTime <= _hitImmuneTime) {
                yield return null;
            }
            graphic.SetHitEffect(false);
            // Hit �鿪 ���� ����
            _isHit = false;
        }
    }
}
