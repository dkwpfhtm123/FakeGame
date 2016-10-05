using UnityEngine;
using System.Collections;

namespace Fake
{
    namespace Rain
    {
        public class ParentBullet : MonoBehaviour
        {
            public GameObject WhiteBullet;
            public GameObject BlueBullet;

            public enum FireType
            {
                White,
                Blue
            }

            public Vector2 Direction
            {
                get;
                private set;
            }

            public float ObjectSpeed
            {
                get;
                private set;
            }

            private bool firing;
            private FireType type;

            private Transform transformCache;

            void Start()
            {
                transformCache = GetComponent<Transform>();
                firing = false;
            }

            void Update()
            {
                if (Manager.Instance.StopTime == false)
                {
                    MoveBullet();
                    if (firing == false)
                    {
                        StartCoroutine(CreateBullet());
                    }
                }
            }

            public void SetUp(Vector2 direction, float objectSpeed, FireType type)
            {
                Direction = direction;
                ObjectSpeed = objectSpeed;
                this.type = type;
            }

            private void MoveBullet()
            {
                Vector2 position = transformCache.localPosition;
                position += Direction * ObjectSpeed * Time.deltaTime;
                transform.localPosition = position;
            }

            private IEnumerator CreateBullet() // 탄 생성 및 발사
            {
                firing = true;
                bool Up = true;

                for (int i = 0; i < 2; i++)
                {
                    GameObject bullet = null;
                    if (type == FireType.White)
                    {
                        bullet = Instantiate(WhiteBullet);
                    }
                    else if (type == FireType.Blue)
                    {
                        bullet = Instantiate(BlueBullet);
                    }
                    ChildBullet setBullet = bullet.GetComponent<ChildBullet>();
                    Transform bulletTransform = bullet.GetComponent<Transform>(); // bullet.transform = bullet.GetComponent<Transform>() 같은 의미 이므로 원하는대로 쓰면 됨.

                    bulletTransform.SetParent(transformCache, false); // bulletTransform.parent == bulletTransform.SetParent(transformCache , true);  // 두번째 인자가 true면 월드좌표를 고정하고 로컬좌표를 움직인다. / false면 로컬좌표를 고정하고 월드좌표를 움직인다.
                    bulletTransform.localPosition = Vector3.zero;
                    bulletTransform.localRotation = Quaternion.identity;
                    bulletTransform.localScale = Vector2.one;

                    if (Up == true)
                    {
                        setBullet.SetUp(GlobalClass.RotateDirection(Direction, 90.0f), 1.0f);
                    }
                    else
                    {
                        setBullet.SetUp(GlobalClass.RotateDirection(Direction, -90.0f), 1.0f);
                    }

                    Up = false;
                }

                yield return new WaitForSeconds(0.2f);
                firing = false;
            }

            void OnCollisionEnter2D(Collision2D coll)
            {
                if (coll.gameObject.GetComponent<Wall>() != null)
                {
                    transformCache.DetachChildren();
                    Destroy(gameObject);
                }
            }
        }
    }
}