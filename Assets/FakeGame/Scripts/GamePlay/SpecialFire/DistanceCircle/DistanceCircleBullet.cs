using UnityEngine;
using System.Collections;

namespace Fake.DistanceCircle
{
    public class DistanceCircleBullet : MonoBehaviour
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
