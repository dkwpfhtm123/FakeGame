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

        //   class Square 잘 모르겠음.

        private Vector2 insideLeftUp;
        private Vector2 insideRightUp;
        private Vector2 insideLeftDown;
        private Vector2 insideRightDown;
        private float insideLength;

        private Vector2 outsideLeftUp;
        private Vector2 outsideRightUp;
        private Vector2 outsideLeftDown;
        private Vector2 outsideRightDown;
        private float outsideLength;

        private Transform transformCache;

        void Start()
        {
            transformCache = GetComponent<Transform>();

            insideLength = 2.0f;
            float insideHalf = insideLength * 0.5f;

            Vector2 position = transformCache.localPosition;

            insideLeftUp = new Vector2(position.x - insideHalf, position.y + insideHalf);
            insideLeftDown = new Vector2(position.x - insideHalf, position.y - insideHalf);
            insideRightUp = new Vector2(position.x + insideHalf, position.y + insideHalf);
            insideRightDown = new Vector2(position.x + insideHalf, position.y - insideHalf);

            outsideLength = insideLength * 2;
            float outsideHalf = outsideLength * 0.5f;

            outsideLeftUp = new Vector2(position.x - outsideHalf, position.y + outsideHalf);
            outsideLeftDown = new Vector2(position.x - outsideHalf, position.y - outsideHalf);
            outsideRightUp = new Vector2(position.x + outsideHalf, position.y + outsideHalf);
            outsideRightDown = new Vector2(position.x + outsideHalf, position.y - outsideHalf);

            StartCoroutine(CreateBullet());
        }

        IEnumerator CreateBullet()
        {
            int angle = 0;
            int anglePlus = 0;
            bool plus = true;

            Vector2 teleportPoint;
            Vector2 crossPoint;
            Side sideOption;

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
                    bulletTransform.localScale = Vector2.one * 0.5f; // 설정이 잘 안됨

                    Vector2 startPoint = transformCache.localPosition;

                    if (45 < angle && angle <= 135)
                    {
                        sideOption = Side.Up;
                        crossPoint = FindCrossPoint(startPoint, startPoint + direction, insideLeftUp, insideRightUp); // 위 
                        teleportPoint = FindTeleportPoint(crossPoint.x, crossPoint.y, sideOption);
                        bullet.SetUp(direction, 0.5f, crossPoint, teleportPoint, Side.Up, angle);
                    }
                    else if (135 < angle && angle <= 225)
                    {
                        sideOption = Side.Left;
                        crossPoint = FindCrossPoint(startPoint, startPoint + direction, insideLeftUp, insideLeftDown); // 왼쪽 
                        teleportPoint = FindTeleportPoint(crossPoint.x, crossPoint.y, sideOption);
                        bullet.SetUp(direction, 0.5f, crossPoint, teleportPoint, Side.Left, angle);
                    }
                    else if (225 < angle && angle <= 315)
                    {
                        sideOption = Side.Down;
                        crossPoint = FindCrossPoint(startPoint, startPoint + direction, insideRightDown, insideLeftDown); // 아래
                        teleportPoint = FindTeleportPoint(crossPoint.x, crossPoint.y, sideOption);
                        bullet.SetUp(direction, 0.5f, crossPoint, teleportPoint, Side.Down, angle);
                    }
                    else
                    {
                        sideOption = Side.Right;
                        crossPoint = FindCrossPoint(startPoint, startPoint + direction, insideRightDown, insideRightUp); // 오른쪽
                        teleportPoint = FindTeleportPoint(crossPoint.x, crossPoint.y, sideOption);
                        bullet.SetUp(direction, 0.5f, crossPoint, teleportPoint, Side.Right, angle);
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

            if (da * db <= 0) // 교차한다. 교차점을 구한다.
            {
                float division = (StraightA.x - StraightB.x) * (LineC.y - LineD.y) - (StraightA.y - StraightB.y) * (LineC.x - LineD.x);

                float dx = AxBy_AyBx(StraightA, StraightB) * (LineC.x - LineD.x) - (StraightA.x - StraightB.x) * AxBy_AyBx(LineC, LineD);
                dx /= division;

                float dy = AxBy_AyBx(StraightA, StraightB) * (LineC.y - LineD.y) - (StraightA.y - StraightB.y) * AxBy_AyBx(LineC, LineD);
                dy /= division;

                crossPoint = new Vector2(dx, dy); // 교차점 좌표.
            }
            else
                crossPoint = Vector2.zero;

            return crossPoint;
        }

        private Vector2 FindTeleportPoint(float dx, float dy, Side sideOption)
        {
            Vector2 teleportPoint;
            float dis; // 거리 비율

            // LeftDown , RightUp 으로 수정해보기. (--) (++)
            if (sideOption == Side.Up)  // 위
            {
                dis = (dx - insideLeftUp.x) / insideLength; // 비율 = 좌표거리 / 총거리 - Y 좌표는 같으므로 X좌표만 계산. (직사각형 이라는 가정에서만 가능)
                teleportPoint = new Vector2(outsideLeftUp.x + outsideLength * dis, outsideLeftUp.y); // 텔포지점 = 원래좌표 C.x + 비율 * 거리
            }
            else if (sideOption == Side.Left) // 왼쪽
            {
                dis = (insideLeftUp.y - dy) / insideLength; // LeftUp 으로부터 계산
                teleportPoint = new Vector2(outsideLeftUp.x, outsideLeftUp.y - outsideLength * dis); // 텔포지점 = 원래좌표 C.y - 비율 * 거리
            }
            else if (sideOption == Side.Down) // 아래
            {
                dis = (insideRightDown.x - dx) / insideLength; // RightDown 으로부터 계산
                teleportPoint = new Vector2(outsideRightDown.x - outsideLength * dis, outsideRightDown.y); // 텔포지점 = 원래좌표 C.x - 비율 * 거리
            }
            else // 오른쪽
            {
                dis = (dy - insideRightDown.y) / insideLength; // RightDown 으로부터 계산
                teleportPoint = new Vector2(outsideRightDown.x, outsideRightDown.y + outsideLength * dis);
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
