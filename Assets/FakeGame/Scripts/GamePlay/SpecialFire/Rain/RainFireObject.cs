using UnityEngine;
using System.Collections;

namespace Fake.Rain
{
    public class RainFireObject : MonoBehaviour
    {
        public enum SpawnDirection
        {
            Left,
            Right
        }

        public GameObject Object;
        public SpawnDirection SpawnObjectDirection;

        private Transform transformCache;
        private Vector2 direction;

        private bool spawning;

        void Start()
        {
            transformCache = GetComponent<Transform>();
            spawning = false;
        }

        void Update()
        {
            if (RainManager.Instance.StopTime == false && spawning == false)
            {
                StartCoroutine(SpawnObject());
            }
        }

        private IEnumerator SpawnObject()
        {
            spawning = true;
            while (RainManager.Instance.StopTime == false)
            {
                GameObject fireObject = Instantiate(Object);
                RainParentBullet setObject = fireObject.GetComponent<RainParentBullet>();
                Transform objectTransform = fireObject.transform;

                if (SpawnObjectDirection == SpawnDirection.Left)
                {
                    setObject.SetUp(Vector2.left, Random.Range(0.9f, 1.1f), RainParentBullet.FireType.White);
                }
                else
                {
                    setObject.SetUp(Vector2.right, Random.Range(0.9f, 1.1f), RainParentBullet.FireType.Blue);
                }

                Vector3 random = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);

                objectTransform.localPosition = transformCache.localPosition + random;
                objectTransform.localRotation = Quaternion.identity;
                objectTransform.localScale = Vector3.one;

                yield return new WaitForSeconds(Random.Range(0.95f, 1.05f));
            }
            spawning = false;
        }
    }
}