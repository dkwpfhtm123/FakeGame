using UnityEngine;
using System.Collections;

public class TPFireObject : MonoBehaviour
{
    public GameObject Bullet;

    private enum side
    {
        Up,
        Down,
        Left,
        Right
    }

    private side sideOption;

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

    private Vector2 crossPoint;
    private Vector2 teleportPoint;

    private Transform transformCache;

    private Vector2 direction;

    void Start()
    {
        transformCache = GetComponent<Transform>();

        direction = new Vector2(1, 0);

        insideLength = 2.0f;
        float insideHalf = insideLength * 0.5f;

        insideLeftUp = new Vector2(transformCache.localPosition.x - insideHalf, transformCache.localPosition.y + insideHalf);
        insideLeftDown = new Vector2(transformCache.localPosition.x - insideHalf, transformCache.localPosition.y - insideHalf);
        insideRightUp = new Vector2(transformCache.localPosition.x + insideHalf, transformCache.localPosition.y + insideHalf);
        insideRightDown = new Vector2(transformCache.localPosition.x + insideHalf, transformCache.localPosition.y - insideHalf);

        outsideLength = insideLength * 2;
        float outsideHalf = outsideLength * 0.5f;

        outsideLeftUp = new Vector2(transformCache.localPosition.x - outsideHalf, transformCache.localPosition.y + outsideHalf);
        outsideLeftDown = new Vector2(transformCache.localPosition.x - outsideHalf, transformCache.localPosition.y - outsideHalf);
        outsideRightUp = new Vector2(transformCache.localPosition.x + outsideHalf, transformCache.localPosition.y + outsideHalf);
        outsideRightDown = new Vector2(transformCache.localPosition.x + outsideHalf, transformCache.localPosition.y - outsideHalf);

        StartCoroutine(CreateBullet());
    }

    IEnumerator CreateBullet()
    {
        float angle = 0;
        float anglePlus = 0;
        bool plus = true;
        while (true)
        {
            for (int i = 0; i < 12; i++)
            {
                Vector2 realDirection = GlobalClass.RotateDirection(direction, angle).normalized;
                float rotateAngle = -Mathf.Atan2(realDirection.x, realDirection.y) * Mathf.Rad2Deg;

                GameObject bullet = Instantiate(Bullet);
                TPBullet setBullet = bullet.GetComponent<TPBullet>();
                Transform bulletTransform = bullet.GetComponent<Transform>();

                bulletTransform.localPosition = transformCache.localPosition;
                bulletTransform.localRotation = Quaternion.Euler(0, 0, rotateAngle);
                bulletTransform.localScale = Vector2.one * 0.5f;

                if (45 < angle && angle <= 135)
                {
                    sideOption = side.Up;
                    Check(transformCache.localPosition, realDirection, insideLeftUp, insideRightUp); // 위 
                    setBullet.SetVariable(realDirection, 0.5f, crossPoint, teleportPoint, TPBullet.Side.Up, angle);
                }
                else if (135 < angle && angle <= 225)
                {
                    sideOption = side.Left;
                    Check(transformCache.localPosition, realDirection, insideLeftUp, insideLeftDown); // 왼쪽 
                    setBullet.SetVariable(realDirection, 0.5f, crossPoint, teleportPoint, TPBullet.Side.Left, angle);
                }
                else if (225 < angle && angle <= 315)
                {
                    sideOption = side.Down;
                    Check(transformCache.localPosition, realDirection, insideRightDown, insideLeftDown); // 아래
                    setBullet.SetVariable(realDirection, 0.5f, crossPoint, teleportPoint, TPBullet.Side.Down, angle);
                }
                else
                {
                    sideOption = side.Right;
                    Check(transformCache.localPosition, realDirection, insideRightDown, insideRightUp); // 오른쪽
                    setBullet.SetVariable(realDirection, 0.5f, crossPoint, teleportPoint, TPBullet.Side.Right, angle);
                }

                angle += 30;
                angle %= 360;
            }

            if (plus == true)
            {
                anglePlus += 5;
            }
            else
            {
                anglePlus -= 5;
            }

            if(anglePlus > 90)
            {
                plus = false;
            }
            else if(anglePlus < 0)
            {
                plus = true;
            }

            angle = anglePlus;
            angle %= 360;

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Check(Vector2 StraightA, Vector2 StraightB, Vector2 LineC, Vector2 LineD) // 직선점a 직선점b : 선분점c 선분점d
    {
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

            float dis; // 거리 비율

            // LeftDown , RightUp 으로 수정해보기. (--) (++)
            if (sideOption == side.Up)
            {
                dis = (dx - insideLeftUp.x) / insideLength; // 비율 = 좌표거리 / 총거리 - Y 좌표는 같으므로 X좌표만 계산. (직사각형 이라는 가정에서만 가능)
                teleportPoint = new Vector2(outsideLeftUp.x + outsideLength * dis, outsideLeftUp.y); // 텔포지점 = 원래좌표 C.x + 비율 * 거리
            }
            else if (sideOption == side.Left)
            {
                dis = (insideLeftUp.y - dy) / insideLength; // LeftUp 으로부터 계산
                teleportPoint = new Vector2(outsideLeftUp.x, outsideLeftUp.y - outsideLength * dis); // 텔포지점 = 원래좌표 C.y - 비율 * 거리
            }
            else if (sideOption == side.Down)
            {
                dis = (insideRightDown.x - dx) / insideLength; // RightDown 으로부터 계산
                teleportPoint = new Vector2(outsideRightDown.x - outsideLength * dis, outsideRightDown.y); // 텔포지점 = 원래좌표 C.x - 비율 * 거리
            }
            else if (sideOption == side.Right)
            {
                dis = (dy - insideRightDown.y) / insideLength; // RightDown 으로부터 계산
                teleportPoint = new Vector2(outsideRightDown.x, outsideRightDown.y + outsideLength * dis);
            }

            //      Debug.Log(crossPoint.x + " " + crossPoint.y);
            //        Debug.Log(teleportPoint.x + " " + teleportPoint.y);
        }
    }

    private float AxBy_AyBx(Vector2 a, Vector2 b) // 복잡한거 함수로 만듬.
    {
        return (a.x * b.y - a.y * b.x);
    }
}
