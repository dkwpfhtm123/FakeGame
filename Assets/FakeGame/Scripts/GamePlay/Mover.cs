using UnityEngine;
using System.Collections;

namespace Fake
{
    public class Mover : MonoBehaviour
    {
        private Transform transformCache;

        private bool plus = true;

        void Start()
        {
            transformCache = gameObject.GetComponent<Transform>();
        }

        public void SetBezierCurve(Vector2 one, Vector2 two, Vector2 three)
        {
            StartCoroutine(BezierCurve(one, two, three));
        }

        public IEnumerator BezierCurve(Vector2 one, Vector2 two, Vector3 three)
        {
            var t = 0.05f;

            while (true)
            {
                transformCache.localPosition = new Vector2((1 - t) * (1 - t) * one.x + 2 * t * (1 - t) * two.x + t * t * three.x, (1 - t) * (1 - t) * one.y + 2 * t * (1 - t) * two.y + t * t * three.y);

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

                    if (t < 0)
                    {
                        plus = true;
                    }
                }

                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}