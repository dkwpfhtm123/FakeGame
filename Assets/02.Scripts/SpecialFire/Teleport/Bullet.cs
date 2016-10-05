using UnityEngine;
using System.Collections;

namespace Fake
{
    namespace Teleport
    {
        public class Bullet : MonoBehaviour
        {
            public Vector2 Direction
            {
                get;
                private set;
            }

            public float Speed
            {
                get;
                private set;
            }

            public Vector2 CrossPoint
            {
                get;
                private set;
            }

            public Vector2 TeleportPoint
            {
                get;
                private set;
            }

            public float Angle
            {
                get;
                private set;
            }

            private Transform transformCache;

            private float teleportNumber;

            private FireObject.Side sideOption;

            void Start()
            {
                transformCache = GetComponent<Transform>();

                teleportNumber = 0;
            }

            void Update()
            {
                Move();
            }

            private void Move()
            {
                Vector2 position = transformCache.localPosition;

                if (sideOption == FireObject.Side.Up)
                {
                    if (position.y > CrossPoint.y && teleportNumber == 0)
                    {
                        position = Teleport(position);

                    }
                    else if (position.y < CrossPoint.y && teleportNumber == 1)
                    {
                        position = Teleport(position);
                    }
                }
                else if (sideOption == FireObject.Side.Left)
                {
                    if (position.x < CrossPoint.x && teleportNumber == 0)
                    {
                        position = Teleport(position);
                    }
                    else if (position.x > CrossPoint.x && teleportNumber == 1)
                    {
                        position = Teleport(position);
                    }
                }
                else if (sideOption == FireObject.Side.Down)
                {
                    if (position.y < CrossPoint.y && teleportNumber == 0)
                    {
                        position = Teleport(position);
                    }
                    else if (position.y > CrossPoint.y && teleportNumber == 1)
                    {
                        position = Teleport(position);
                    }
                }
                else if (sideOption == FireObject.Side.Right)
                {
                    if (position.x > CrossPoint.x && teleportNumber == 0)
                    {
                        position = Teleport(position);
                    }
                    else if (position.x < CrossPoint.x && teleportNumber == 1)
                    {
                        position = Teleport(position);
                    }
                }

                position += Direction * Speed * Time.deltaTime;

                transformCache.localPosition = position;
            }

            private Vector2 Teleport(Vector2 position)
            {
                Direction = GlobalClass.RotateDirection(Direction, 180.0f);
                float rotateAngle = -Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;
                transformCache.localRotation = Quaternion.Euler(0, 0, rotateAngle);

                transform.localPosition = TeleportPoint;
                position = transformCache.localPosition;

                teleportNumber++;

                return position;
            }

            public void SetUp(Vector2 direction, float speed, Vector2 crossPoint, Vector2 teleportPoint, FireObject.Side sideOption, float angle)
            {
                Direction = direction;
                Speed = speed;
                CrossPoint = crossPoint;
                TeleportPoint = teleportPoint;
                this.sideOption = sideOption;
                Angle = angle;
            }
        }
    }
}
