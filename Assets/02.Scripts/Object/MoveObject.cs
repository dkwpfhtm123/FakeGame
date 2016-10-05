using UnityEngine;
using System.Collections;

namespace Fake
{
    public class MoveObject : MonoBehaviour
    {
        private Transform transformCache;

        void Start()
        {
            transformCache = GetComponent<Transform>();
        }

        public void Set(Vector2 one, Vector2 two, Vector2 three)
        {
            Move(one, two, three);
        }

        public IEnumerator Move(Vector2 one, Vector2 two, Vector3 three)
        {
            float t = 0.05f;

            while (true)
            {
                Vector2 direct;

                direct.x = (1 - t) * (1 - t) * one.x + 2 * t * (1 - t) * two.x + t * t * three.x;
                direct.y = (1 - t) * (1 - t) * one.y + 2 * t * (1 - t) * two.y + t * t * three.y;

                transform.localPosition = direct;

                t += 0.05f;
                t %= 1;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
}