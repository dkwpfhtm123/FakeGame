using UnityEngine;
using System.Collections;

namespace Teleport
{
    public class FireObject : MonoBehaviour
    {
        private FireObject instance;
        public FireObject Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<FireObject>();

                return instance;
            }
        }

        public GameObject Bullet;

        public enum Side
        {
            Up,
            Down,
            Left,
            Right
        }

        public class Square
        {
            public readonly Vector2 LeftUp;
            public readonly Vector2 LeftDown;
            public readonly Vector2 RightUp;
            public readonly Vector2 RightDown;

            public float Length
            {
                get { return RightUp.x - LeftUp.x; }
            }

            public Square(Vector2 position, float length)
            {
                float lengthHalf = length * 0.5f;
                LeftUp = new Vector2(position.x - lengthHalf, position.y + lengthHalf);
                LeftDown = new Vector2(position.x - lengthHalf, position.y - lengthHalf);
                RightUp = new Vector2(position.x + lengthHalf, position.y + lengthHalf);
                RightDown = new Vector2(position.x + lengthHalf, position.y - lengthHalf);
            }
        }

        private Square inside;
        private Square outside;

        private Transform transformCache;

        void Start()
        {
            transformCache = GetComponent<Transform>();

            Vector2 position = transformCache.localPosition;

            inside = new Square(position, 2.0f);
            outside = new Square(position, inside.Length * 2.0f);

            StartCoroutine(CreateBullet());
        }

        IEnumerator CreateBullet()
        {
            int angle = 0;
            int anglePlus = 0;
            bool plus = true;

            while (true)
            {
                for (int i = 0; i < 12; i++)
                {
                    Vector2 direction = GlobalClass.RotateDirection(new Vector2(1, 0), angle).normalized;
                    float rotateAngle = -Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

                    GameObject bulletObject = Instantiate(Bullet);
                    Bullet bullet = bulletObject.GetComponent<Bullet>();
                    Transform bulletTransform = bulletObject.GetComponent<Transform>();

                    bulletTransform.localPosition = transformCache.localPosition;
                    bulletTransform.localRotation = Quaternion.Euler(0, 0, rotateAngle);
                    //         bulletTransform.localScale = Vector2.one;

                    Vector2 startPoint = transformCache.localPosition;

                    if (45 < angle && angle <= 135)
                    {
                        Side sideOption = Side.Up;
                        Vector2 crossPoint = FindCrossPoint(startPoint, startPoint + direction, inside.LeftUp, inside.RightUp); // 위 
                        Vector2 teleportPoint = FindTeleportPoint(crossPoint.x, crossPoint.y, sideOption);
                        bullet.SetUp(direction, 0.5f, crossPoint, teleportPoint, sideOption, angle);
                    }
                    else if (135 < angle && angle <= 225)
                    {
                        Side sideOption = Side.Left;
                        Vector2 crossPoint = FindCrossPoint(startPoint, startPoint + direction, inside.LeftUp, inside.LeftDown); // 왼쪽 
                        Vector2 teleportPoint = FindTeleportPoint(crossPoint.x, crossPoint.y, sideOption);
                        bullet.SetUp(direction, 0.5f, crossPoint, teleportPoint, sideOption, angle);
                    }
                    else if (225 < angle && angle <= 315)
                    {
                        Side sideOption = Side.Down;
                        Vector2 crossPoint = FindCrossPoint(startPoint, startPoint + direction, inside.RightDown, inside.LeftDown); // 아래
                        Vector2 teleportPoint = FindTeleportPoint(crossPoint.x, crossPoint.y, sideOption);
                        bullet.SetUp(direction, 0.5f, crossPoint, teleportPoint, sideOption, angle);
                    }
                    else
                    {
                        Side sideOption = Side.Right;
                        Vector2 crossPoint = FindCrossPoint(startPoint, startPoint + direction, inside.RightDown, inside.RightUp); // 오른쪽
                        Vector2 teleportPoint = FindTeleportPoint(crossPoint.x, crossPoint.y, sideOption);
                        bullet.SetUp(direction, 0.5f, crossPoint, teleportPoint, sideOption, angle);
                    }

                    angle += 30;
                    angle %= 360;
                }

                if (plus == true)
                    anglePlus += 5;
                else
                    anglePlus -= 5;

                if (anglePlus > 90)
                    plus = false;
                else if (anglePlus < 0)
                    plus = true;

                angle = anglePlus;
                angle %= 360;

                yield return new WaitForSeconds(0.5f);
            }
        }

        private static Vector2 FindCrossPoint(Vector2 StraightA, Vector2 StraightB, Vector2 LineC, Vector2 LineD) // 직선점a 직선점b : 선분점c 선분점d
        {
            Vector2 crossPoint;

            float da = (StraightA.x - StraightB.x) * (LineC.y - StraightA.y) + (StraightA.y - StraightB.y) * (StraightA.x - LineC.x);
            float db = (StraightA.x - StraightB.x) * (LineD.y - StraightA.y) + (StraightA.y - StraightB.y) * (StraightA.x - LineD.x);

            //         if (da * db <= 0) // 교차한다. 교차점을 구한다.
            //     {
            float division = (StraightA.x - StraightB.x) * (LineC.y - LineD.y) - (StraightA.y - StraightB.y) * (LineC.x - LineD.x);

            float dx = AxBy_AyBx(StraightA, StraightB) * (LineC.x - LineD.x) - (StraightA.x - StraightB.x) * AxBy_AyBx(LineC, LineD);
            dx /= division;

            float dy = AxBy_AyBx(StraightA, StraightB) * (LineC.y - LineD.y) - (StraightA.y - StraightB.y) * AxBy_AyBx(LineC, LineD);
            dy /= division;

            crossPoint = new Vector2(dx, dy); // 교차점 좌표.
                            //       }
                            //        else
                            //           crossPoint = Vector2.zero;

            return crossPoint;
        }

        private Vector2 FindTeleportPoint(float dx, float dy, Side sideOption)
        {
            Vector2 teleportPoint;
            float dis; // 거리 비율

            // LeftDown , RightUp 으로 수정해보기. (--) (++)
            if (sideOption == Side.Up)  // 위
            {
                dis = (dx - inside.LeftUp.x) / inside.Length; // 비율 = 좌표거리 / 총거리 - Y 좌표는 같으므로 X좌표만 계산. (직사각형 이라는 가정에서만 가능)
                teleportPoint = new Vector2(outside.LeftUp.x + outside.Length * dis, outside.LeftUp.y); // 텔포지점 = 원래좌표 C.x + 비율 * 거리
            }
            else if (sideOption == Side.Left) // 왼쪽
            {
                dis = (inside.LeftUp.y - dy) / inside.Length; // LeftUp 으로부터 계산
                teleportPoint = new Vector2(outside.LeftUp.x, outside.LeftUp.y - outside.Length * dis); // 텔포지점 = 원래좌표 C.y - 비율 * 거리
            }
            else if (sideOption == Side.Down) // 아래
            {
                dis = (inside.RightDown.x - dx) / inside.Length; // RightDown 으로부터 계산
                teleportPoint = new Vector2(outside.RightDown.x - outside.Length * dis, outside.RightDown.y); // 텔포지점 = 원래좌표 C.x - 비율 * 거리
            }
            else // 오른쪽
            {
                dis = (dy - inside.RightDown.y) / inside.Length; // RightDown 으로부터 계산
                teleportPoint = new Vector2(outside.RightDown.x, outside.RightDown.y + outside.Length * dis);
            }

            return teleportPoint;
        }

        private static float AxBy_AyBx(Vector2 a, Vector2 b) // 복잡한거 함수로 만듬.
        {
            return (a.x * b.y - a.y * b.x);
        }

        void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }
}
