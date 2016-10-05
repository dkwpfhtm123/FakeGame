using UnityEngine;
using System.Collections;

namespace Fake
{
    namespace DistanceCirecle
    {
        public class Bullet : MonoBehaviour
        {
            void Start()
            {
                StartCoroutine(TimeOut());
            }

            IEnumerator TimeOut()
            {
                yield return new WaitForSeconds(4.0f);
                Destroy(gameObject);
            }
        }
    }
}
