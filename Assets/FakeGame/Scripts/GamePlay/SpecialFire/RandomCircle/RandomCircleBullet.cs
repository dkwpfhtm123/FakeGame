using UnityEngine;
using System.Collections;

namespace Fake.RandomCircle
{
    public class RandomCircleBullet : MonoBehaviour
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
