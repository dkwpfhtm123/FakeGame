using UnityEngine;
using System.Collections;

namespace Fake
{
    namespace RandomCircle
    {
        public class Bullet : MonoBehaviour
        {
            void Start()
            {
                StartCoroutine(Destroy());
            }

            IEnumerator Destroy()
            {
                yield return new WaitForSeconds(10.0f);
                Destroy(gameObject);
            }
        }
    }
}
