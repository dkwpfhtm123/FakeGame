using UnityEngine;
using System.Collections;

namespace Fake
{
    namespace LineSpread
    {
        public class FireObject : MonoBehaviour
        {
            public GameObject BulletA;
            public GameObject BulletB;

            private bool firstFire;
            private bool firing;

            private Transform transformCache;
            private Transform playerTransform;

            void Start()
            {
                transformCache = GetComponent<Transform>();

                firstFire = false;
                firing = false;
            }

            void Update()
            {
                if (firing == false && Manager.Instance.WaitTime == false)
                {
                    StartCoroutine(FireBullet());
                }
            }

            IEnumerator FireBullet()
            {
                firing = true; // object에서 bullet을 발사하는중.
                Manager.Instance.WaitTime = true; // bullet에서 bullet을 발사하는 일이 끝날 때까지 대기.

                yield return new WaitForSeconds(0.1f);
                int anglePlus = 0;

                playerTransform = Fake.GameMgr.Instance.PlayerTransform;
                Vector2 targetVector = (playerTransform.localPosition - transformCache.localPosition).normalized;

                while (Manager.Instance.OnCollision == false) // bullet이 충돌할 때까지 무한반복
                {
                    yield return new WaitForSeconds(0.1f); // 발사간격
                    for (int i = 0; i < 6; i++)
                    {
                        GameObject bulletObject;
                        if (firstFire == false)
                        {
                            bulletObject = Instantiate(BulletA);
                            firstFire = true; // 첫번째만 발사
                        }
                        else
                        {
                            bulletObject = Instantiate(BulletB);
                        }
                        Transform bulletTransform = bulletObject.GetComponent<Transform>();
                        Bullet bullet = bulletObject.GetComponent<Bullet>();

                        bulletTransform.parent = transformCache;

                        bulletTransform.localPosition = Vector3.zero;
                        bulletTransform.localRotation = Quaternion.identity;
                        bulletTransform.localScale = Vector3.one * 0.5f;

                        bullet.SetUp(targetVector, 3.0f, anglePlus, false);

                        anglePlus = (i + 1) * 60; // 60도씩 돌림.
                    }
                    anglePlus = 0;
                }
                firing = false; // object에서 bullet을 발사하기를 끝냄. 
                firstFire = false; // 첫번째만 발사 초기화
            }
        }
    }
}