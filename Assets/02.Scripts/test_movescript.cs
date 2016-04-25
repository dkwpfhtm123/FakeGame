using UnityEngine;
using System.Collections;

public class test_movescript : MonoBehaviour
{
    //? 프로퍼티로 구현해보기
    //? 초기화 함수도 만들기
    private Vector2 direction;
    private float bulletSpeed;
    private bool makingBullet;
    private float angle;
    public int life;

    private Transform transformCache;
    private bool creating;

    void Start()
    {
        transformCache = GetComponent<Transform>();
        creating = false;

        RotateBullet();
    }

    void Update()
    {
        if (test_managerscript.Instance.OnCollision == false)
        {
            MoveBullet();
        }
        else
        {
            if (MakingBullet == false)
            {
                if (creating == false)
                {
                    StartCoroutine(CreateBullet());
                }
            }
        }

        if (MakingBullet == true)
        {
            MoveBullet();
        }
    }

    void MoveBullet()
    {
        if (transformCache == null)
        {
            transformCache = GetComponent<Transform>();
        }

        //? xy 나누지 말기
        Vector2 position = transformCache.localPosition;
        position.x += Direction.x * BulletSpeed * Time.deltaTime;
        position.y += Direction.y * BulletSpeed * Time.deltaTime;

        transformCache.localPosition = position;
    }

    //? 액세스 한정자 지정하자
    void RotateBullet()
    {
        Vector2 targetDirection = Direction;
        float radian = Angle * Mathf.Deg2Rad;

        //? 통합하기 확장 메서드
        Direction.x = targetDirection.x * Mathf.Cos(radian) - targetDirection.y * Mathf.Sin(radian);
        Direction.y = targetDirection.x * Mathf.Sin(radian) + targetDirection.y * Mathf.Cos(radian);
    }

    /*   private static Vector2 RotateVector2ByDegree(Vector2 original, float degree)
      {
      } */

    public void Setup(Vector2 direction, float bulletSpeed, bool makingBullet, float angle, int life = 10)
    {
        this.direction = direction.normalized;
        this.bulletSpeed = Mathf.Max(bulletSpeed, 0.0f);
        this.makingBullet = makingBullet;
        this.angle = angle;
    }

    IEnumerator CreateBullet()
    {
        for (int i = 0; i < 3; i++)
        {
            test_managerscript.Instance.WaitTime = true;
            creating = true;

            GameObject bullet = Instantiate(gameObject);
            Transform bulletTransform = bullet.GetComponent<Transform>();
            test_movescript bulletScript = bullet.GetComponent<test_movescript>();

            bulletTransform.localPosition = transformCache.localPosition;
            bulletTransform.localRotation = Quaternion.identity;
            bulletTransform.localScale = Vector3.one * 0.5f;

            bulletScript.BulletSpeed = 3.0f;
            bulletScript.Angle = Random.Range(0, 360.0f);
            bulletScript.Direction = Direction;
            bulletScript.MakingBullet = true;

            yield return new WaitForSeconds(1.0f); // 1초마다 한번씩 호출 x3

            creating = false;
        }
        Angle = Random.Range(0, 360);
        RotateBullet();

        test_managerscript.Instance.OnCollision = false;
        yield return new WaitForSeconds(2.0f);
        test_managerscript.Instance.WaitTime = false;
    }
    
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (gameObject.GetComponent<NoWallDestroy>())
        {
            if (coll.gameObject.GetComponent<Wall>()) //? null 체크
            {
                test_managerscript.Instance.OnCollision = true;
                Destroy(gameObject);
            }
        }
    }
}
