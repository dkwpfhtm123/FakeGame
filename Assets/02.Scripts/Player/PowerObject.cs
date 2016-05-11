using UnityEngine;
using System.Collections;

namespace Player
{
    public class PowerObject : MonoBehaviour
    {
        private Transform transformCache;

        public float Angle;
        public float Radius;

        void Update()
        {
            if (GameMgr.Instance.RespawnPlayer == true)
            {
                Destroy(gameObject);
            }
        }

        public void StartRotatePower(Transform playerTransform)
        {
            transformCache = GetComponent<Transform>();
            StartCoroutine(RotatePower(playerTransform));
        }

        private IEnumerator RotatePower(Transform playerTransform)
        {
            while (GameMgr.Instance.RespawnPlayer == false)
            {
                Vector2 circle = playerTransform.localPosition;

                circle.x = playerTransform.localPosition.x + (Mathf.Sin(Angle * Mathf.Deg2Rad) * Radius);
                circle.y = playerTransform.localPosition.y + (Mathf.Cos(Angle * Mathf.Deg2Rad) * Radius);

                transformCache.localPosition = circle;

                Angle += 180 * Time.deltaTime;
                Angle = Angle % 360.0f;

                yield return null; // 프레임마다 반복
            }
        }
    }
}