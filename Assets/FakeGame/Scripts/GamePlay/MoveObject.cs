using UnityEngine;
using System.Collections;

namespace Fake
{
    public class MoveObject : MonoBehaviour
    {
        private Transform transformCache;

        public Vector2 dir;

        private bool plus = true;

        void Start()
        {
            transformCache = gameObject.GetComponent<Transform>();
        }

        public void Set(Vector2 one, Vector2 two, Vector2 three)
        {
            StartCoroutine(Move(one, two, three));
        }

        public IEnumerator Move(Vector2 one, Vector2 two, Vector3 three)
        {
            float t = 0.05f;
            Vector2 direct;

            while (true)
            {
                direct.x = (1 - t) * (1 - t) * one.x + 2 * t * (1 - t) * two.x + t * t * three.x;
                direct.y = (1 - t) * (1 - t) * one.y + 2 * t * (1 - t) * two.y + t * t * three.y;

                transformCache.localPosition = direct;

                if (plus == true)
                {
                    t += 0.01f;

                    if (t > 1)
                    {
                        plus = false;
                    }
                }
                else
                {
                    t -= 0.01f;

                    if(t < 0)
                    {
                        plus = true;
                    }
                }

                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}