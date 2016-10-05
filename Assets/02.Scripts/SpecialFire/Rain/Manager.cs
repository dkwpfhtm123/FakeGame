using UnityEngine;
using System.Collections;

namespace Fake
{
    namespace Rain
    {
        public class Manager : MonoBehaviour
        {
            private static Manager instance;
            public static Manager Instance
            {
                get
                {
                    if (instance == null)
                        instance = FindObjectOfType<Manager>();

                    return instance;
                }
            }

            public bool StopTime;

            void Start()
            {
                StopTime = false;

                StartCoroutine(TimeWatch());
            }

            void OnDestroy()
            {
                if (instance == this)
                    instance = null;
            }

            private IEnumerator TimeWatch()
            {
                while (true)
                {
                    yield return new WaitForSeconds(3.0f);
                    StopTime = true;

                    yield return new WaitForSeconds(1.5f);
                    StopTime = false;
                }
            }
        }
    }
}