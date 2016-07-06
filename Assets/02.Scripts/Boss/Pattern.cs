using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Boss
{
    public class Pattern : MonoBehaviour
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
            GameObject WaitTimeBoss = MakeBossImage();

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
            Destroy(gameObject);
        }

        private GameObject MakeBossImage()
        {
            GameObject WaitTimeBoss = Instantiate(BossImageObject);
            Transform bossTransform = WaitTimeBoss.GetComponent<Transform>();

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

            GameObject patternObject = Instantiate(pattern[number]);
            Transform objectTransform = patternObject.GetComponent<Transform>();

            objectTransform.localPosition = transformCache.localPosition;
            objectTransform.localRotation = Quaternion.identity;
            objectTransform.localScale = Vector3.one;

            return patternObject;
        }

        void OnCollisionEnter2D(Collision2D coll)
        {
       //     if (patternStart == true)
         //   {
                if (coll.gameObject.GetComponent<BulletTypeScript>() != null)
                {
                    BulletTypeScript bulletType = coll.gameObject.GetComponent<BulletTypeScript>();

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