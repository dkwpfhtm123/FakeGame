using UnityEngine;
using System.Collections;

namespace RandomCircle
{
    public class Object : MonoBehaviour
    {
        public GameObject Bullet;

        private Transform transformCache;

        void Start()
        {
            transformCache = GetComponent<Transform>();

            StartCoroutine(CreateBullet());
        }

        IEnumerator CreateBullet()
        {
            Vector2 direction = new Vector2(1, 0);

                int degree = 0; // 꽃잎설정
            while (true)
            {
            //    float angle = Random.Range(0.0f, 360.0f); // 원 설정
              //  float distance = Random.Range(0.0f, 1.0f); // min 값만 0으로 수정하면 원 안의 랜덤범위.

                       float radian = degree * Mathf.Deg2Rad; // 꽃잎설정
                       float distance = 2 * Mathf.Sin(4 * radian); ; // 꽃잎설정
                      Vector2 position = Change(Vector3.zero , distance, degree); // 꽃잎설정

             //   Vector2 position = Change(transformCache.localPosition, distance, angle);  // 원 설정

                GameObject bulletObject = Instantiate(Bullet);
                Transform bulletTransform = bulletObject.GetComponent<Transform>();

                bulletTransform.parent = transformCache;

                bulletTransform.localPosition = position;
                bulletTransform.localRotation = Quaternion.identity;
                bulletTransform.localScale = Vector2.one;

                   degree += 5; // 꽃잎설정
                   degree %= 360; // 꽃잎설정
                yield return new WaitForSeconds(0.1f);
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