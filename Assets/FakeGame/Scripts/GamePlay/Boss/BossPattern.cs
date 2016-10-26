using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Fake.Boss
{
    public class BossPattern : MonoBehaviour
    {
        public GameObject BossImageObject; // 이미지만 같은 오브젝트
        public GameObject[] pattern;
        public Image bossHPbar;

        private Transform transformCache;

        private GameObject BossTransformObject;

        private int currentPattern;

        private int bossLife;
        private float currentBossHP;
        private float bossHPMax;

        private GameObject bossPattern;

        private bool patternStart;

        //     private float timeRemainings;
        //     private System.Action timeAction;

        void Start()
        {
            transformCache = GetComponent<Transform>();

            var hpbar = Instantiate(bossHPbar);
            hpbar.transform.localPosition = new Vector2(835, -35.5f);
            hpbar.transform.localScale = Vector2.one;

            bossLife = pattern.Length;
            currentPattern = 0;

            patternStart = false;

            StartCoroutine(StartPattern());
        }

        void Update()
        {
            if (patternStart == true)
                transformCache.localPosition = BossTransformObject.transform.localPosition;
        }

        private IEnumerator StartPattern()
        {
            GetComponent<Collider2D>().enabled = false;
            var WaitTimeBoss = MakeBossImage();

            // 패턴 시작 직전 패턴이름, 패턴애니메이션 기타등등 넣을곳
            yield return new WaitForSeconds(3.0f);

            bossPattern = FirePattern(currentPattern);
            BossTransformObject = bossPattern;

            patternStart = true;

            Destroy(WaitTimeBoss);
        }

        private void ChangePattern()
        {
            patternStart = false;

            Destroy(bossPattern);
            currentPattern++;

            StartCoroutine(StartPattern());
        }

        private void DeadBoss()
        {
            Destroy(bossPattern);
            Debug.Log("BossDead!");
            //      Destroy(gameObject);
        }

        private GameObject MakeBossImage()
        {
            var WaitTimeBoss = Instantiate(BossImageObject);
            var bossTransform = WaitTimeBoss.GetComponent<Transform>();

            bossTransform.localPosition = transformCache.localPosition;
            bossTransform.localRotation = Quaternion.identity;
            bossTransform.localScale = Vector3.one;

            return WaitTimeBoss;
        }

        private GameObject FirePattern(int number)
        {
            bossHPMax = 30;
            currentBossHP = bossHPMax;
            bossHPbar.fillAmount = currentBossHP / bossHPMax;

            GetComponent<Collider2D>().enabled = true;

            var patternObject = Instantiate(pattern[number]);
            var objectTransform = patternObject.GetComponent<Transform>();

            objectTransform.localPosition = transformCache.localPosition;
            objectTransform.localRotation = Quaternion.identity;
            objectTransform.localScale = Vector3.one;

            return patternObject;
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
            //     if (patternStart == true)
            //   {
            if (coll.gameObject.GetComponent<BaseBullet>() != null)
            {
                var bulletType = coll.gameObject.GetComponent<BaseBullet>();

                if (bulletType.BulletTypeCheck == BulletType.PlayerBullet)
                {
                    Destroy(coll.gameObject);
                    currentBossHP--;
                    bossHPbar.fillAmount = currentBossHP / bossHPMax;

                    if (currentBossHP < 0)
                    {
                        bossLife--;
                        if (bossLife > 0)
                        {
                            ChangePattern();
                        }
                        else
                        {
                            DeadBoss();
                        }
                    }
                }
                //    }
            }
        }
    }
}