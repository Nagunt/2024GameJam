using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class Weather : MonoBehaviour
    {
        public enum State
        {
            none,
            thunder,
            hurricane,
            cold,
            hot
        }

        public float timer;
        public float tiktok;

        public GameObject[] weatherPrefab;

        private void Update()
        {
            tiktok += Time.deltaTime;
            if(tiktok>3)
            {
                CallWeather(randomState());
                tiktok = 0;
            }
        }

        State randomState()
        {
            int randomNum = Random.Range(0, 2);
            if (randomNum == 0)
            {
                return State.thunder;
            }
            else if (randomNum == 1)
            {
                return State.hurricane;
            }
            else if (randomNum == 2)
            {
                return State.cold;
            }
            else if (randomNum == 3)
            {
                return State.hot;
            }
            else
            {
                return State.none;
            }
        }

        void CallWeather(State state)
        {
            if(state == State.thunder)
            {
                for(int i=0; i<3; i++)
                {
                    Vector2 playerVector = GameJam.Player.Instance.gameObject.transform.position;
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(playerVector + new Vector2(Random.Range(-5, 5),3),new Vector2(0,-1));
                    if(raycastHit2D.collider!=null&& raycastHit2D.collider.gameObject.layer==6&& raycastHit2D.collider.gameObject.tag!="Player")
                    {
                        /*
                        GameObject point = Instantiate<GameObject>(weatherPrefab[0], raycastHit2D.point, Quaternion.identity);
                        timer += Time.deltaTime;
                        
                        GameObject go = Instantiate<GameObject>(weatherPrefab[1], raycastHit2D.point, Quaternion.identity);
                        go.GetComponent<Thunder>().thunderGraphic.SetTrigger();
                        Destroy(point, 1f);
                        Destroy(go, 1f);
                        */
                        StartCoroutine(ThunderCoroutine());


                    }
 
                }
            }
            else if(state == State.hurricane)
            {
                Vector2 playerVector = GameJam.Player.Instance.gameObject.transform.position;
                RaycastHit2D raycastHit2D = Physics2D.Raycast(playerVector + new Vector2(Random.Range(6, 9), 2), new Vector2(0, -1));
                if (raycastHit2D.collider.gameObject.layer == 6)
                {

                    GameObject go = Instantiate<GameObject>(weatherPrefab[2], raycastHit2D.point, Quaternion.identity);
                    
                }
            }
            else if (state == State.cold)
            {

            }
            else if (state == State.hot)
            {

            }
        }

        IEnumerator ThunderCoroutine()
        {
            Vector2 playerVector = GameJam.Player.Instance.gameObject.transform.position;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(playerVector + new Vector2(Random.Range(-5, 5), 3), new Vector2(0, -1));

            GameObject point = Instantiate<GameObject>(weatherPrefab[0], raycastHit2D.point, Quaternion.identity);
            Destroy(point, 1f);

            yield return new WaitForSecondsRealtime(1f);
            GameObject go = Instantiate<GameObject>(weatherPrefab[1], raycastHit2D.point, Quaternion.identity);
            go.GetComponent<Thunder>().thunderGraphic.SetTrigger();
            Destroy(go, 1f);
            
        }

    }
}
