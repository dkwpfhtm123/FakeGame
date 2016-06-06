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

        private int endPattern;
        private int currentPattern;

        private int currentBossHP;
        private int bossHPMax;

        private bool patternStart;

        void Start()
        {
            transformCache = GetComponent<Transform>();

            endPattern = pattern.Length - 1;
            currentPattern = 0;

            patternStart = false;

            StartCoroutine(PatternStart());
        }

        void Update()
        {
            if (patternStart == true)
                transformCache.localPosition = BossTransformObject.transform.localPosition;
        }

        private IEnumerator PatternStart()
        {
            while (endPattern >= currentPattern)
            {
                GameObject WaitTimeBoss = MakeBossImage();

                // 패턴 시작 직전 패턴이름, 패턴애니메이션 기타등등 넣을곳
                yield return new WaitForSeconds(3.0f);

                GameObject bossPattern = StartPattern(currentPattern);
                BossTransformObject = bossPattern;

                patternStart = true;

                Destroy(WaitTimeBoss);

                yield return new WaitForSeconds(5.0f);

                patternStart = false;

                Destroy(bossPattern);
                currentPattern++;
            }

            Debug.Log("End Pattern");

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

        private GameObject StartPattern(int number)
        {
            bossHPMax = 30;
            currentBossHP = bossHPMax;
            bossHPbar.fillAmount = currentBossHP / bossHPMax;

            GameObject patternObject = Instantiate(pattern[number]);
            Transform objectTransform = patternObject.GetComponent<Transform>();

            objectTransform.localPosition = transformCache.localPosition;
            objectTransform.localRotation = Quaternion.identity;
            objectTransform.localScale = Vector3.one;

            return patternObject;
        }

  /*      void OnCollisionEnter2D(Collider2D coll)
        {
            if (coll.gameObject.GetComponent<BulletTypeScript>() != null)
            {
                BulletTypeScript bulletType = coll.gameObject.GetComponent<BulletTypeScript>();

                if (bulletType.BulletTypeCheck == BulletType.PlayerBullet)
                {
                    Destroy(coll.gameObject);
                    currentBossHP--;
                    bossHPbar.fillAmount = currentBossHP / bossHPMax;
                }
            }
        } */
    }
}