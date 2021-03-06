﻿using UnityEngine;
using System.Collections;

namespace Fake.DistanceCircle
{
    public class DistanceCircleFireObject : MonoBehaviour
    {
        public GameObject Bullet;

        private Transform transformCache;
        private Transform playerTransform;

        int degree;
        float distance;

        void Start()
        {
            transformCache = GetComponent<Transform>();
            playerTransform = Fake.GameManager.Instance.PlayerTransform;

            degree = 0;
            distance = 0;

            StartCoroutine(CreateBullet());
        }

        IEnumerator CreateBullet()
        {
            //       yield return new WaitForSeconds(0.1f); // 이부분 빼는거 질문

            while (true)
            {
                if (playerTransform == null)
                    playerTransform = Fake.GameManager.Instance.PlayerTransform;

                distance = Vector3.Distance(playerTransform.localPosition, transformCache.localPosition);

                if (distance > 5)
                    distance = 5;

                for (int i = 0; i < 4; i++)
                {
                    GameObject bulletObject = Instantiate(Bullet);
                    Transform bullet = bulletObject.GetComponent<Transform>();

                    bullet.parent = transformCache;

                    bullet.localPosition = Change(Vector3.zero, distance, degree);
                    bullet.localRotation = Quaternion.identity;
                    bullet.localScale = Vector2.one * 0.3f;

                    degree += 90;
                }
                yield return new WaitForSeconds(0.1f);
                degree += 3;

                degree %= 360;
            }
        }

        private Vector2 Change(Vector2 startPosition, float dis, float degree)
        {
            float radian = degree * Mathf.Deg2Rad;

            float dx = dis * Mathf.Cos(radian);
            float dy = dis * Mathf.Sin(radian);

            Vector2 result = new Vector2(startPosition.x + dx, startPosition.y + dy);

            return result;
        }
    }
}