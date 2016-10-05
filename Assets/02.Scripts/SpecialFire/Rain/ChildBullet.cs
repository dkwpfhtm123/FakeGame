using UnityEngine;
using System.Collections;

namespace Fake
{
    namespace Rain
    {
        public class ChildBullet : MonoBehaviour
        {
            public Vector2 Direction
            {
                get;
                private set;
            }
            public float BulletSpeed
            {
                get;
                private set;
            }

            private ParentBullet parent;
            private Transform transformCache;

            void Start()
            {
                transformCache = GetComponent<Transform>();
                parent = gameObject.GetComponentInParent<ParentBullet>();
            }

            void Update()
            {
                if (Manager.Instance.StopTime == false)
                {
                    MoveBullet();
                    if (parent != null)
                    {
                        MoveOppositeParent();
                    }
                }
                else {
                    ChangeDirection();
                }
            }

            public void SetUp(Vector2 direction, float bulletSpeed)
            {
                Direction = direction;
                BulletSpeed = bulletSpeed;
            }

            private void MoveOppositeParent() // parent 와 같은속도로 반대방향으로 움직임.
            {
                Vector2 position = transformCache.localPosition;
                Vector2 oppositeParentDirection = GlobalClass.RotateDirection(parent.Direction, -180.0f);

                position += oppositeParentDirection * parent.ObjectSpeed * Time.deltaTime;

                transformCache.localPosition = position;
            }

            private void MoveBullet()
            {
                Vector2 position = transformCache.localPosition;

                position += Direction * BulletSpeed * Time.deltaTime;

                transformCache.localPosition = position;
            }

            private void ChangeDirection()
            {
                Vector2 oldDirection = Direction;
                float oldBulletSpeed = BulletSpeed;

                BulletSpeed = 0.5f;
                Direction = GlobalClass.RotateDirection(parent.Direction, -180.0f);

                MoveBullet();

                Direction = oldDirection;
                BulletSpeed = oldBulletSpeed;
            }
        }
    }
}
