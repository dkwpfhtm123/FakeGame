using UnityEngine;
using System.Collections;

public class test_bullet_1 : MonoBehaviour
{
    //? 프로퍼티로 구현해보기
    //? 초기화 함수도 만들기
    private Vector2 direction;
    public Vector2 Direction
    {
        get
        {
            return direction;
        }
        private set
        {
            direction = value;
        }
    }

    private float bulletSpeed;
    public float BulletSpeed
    {
        get
        {
            return bulletSpeed;
        }
        private set
        {
            bulletSpeed = value;
        }
    }

    private bool madeInBullet;
    public bool MadeInBullet
    {
        get
        {
            return madeInBullet;
        }
        private set
        {
            madeInBullet = value;
        }
    }

    private float angle;
    public float Angle
    {
        get
        {
            return angle;
        }
        private set
        {
            angle = value;
        }
    }

    private Transform transformCache;
    private bool creating;

    void Start()
    {
        transformCache = GetComponent<Transform>();
        creating = false;

        Direction = GlobalMethod.Global.RotateDirection(Direction, angle);
    }

    void Update()
    {
        if (test_manager_1.Instance.OnCollision == false) // 충돌판정이 있기 전까지 움직임.
        {
            MoveBullet();
        }
        else // 충돌판정이 왔을때
        {
            if (MadeInBullet == false) // (bullet에서 만들어진 bullet) , (fireobject에서 만들어진 bullet)  판단.
            {
                if (creating == false)  // bullet에서 bullet을 만들고 있지 않을 때
                {
                    StartCoroutine(CreateBullet());
                }
            }
        }

        if (MadeInBullet == true) // bullet에서 만들어진 bullet 일 때
        {
            MoveBullet();
        }
    }

    public void SetValue(Vector2 direction , float bulletSpeed , float angle , bool madeInBullet)
    {
        this.direction = direction;
        this.bulletSpeed = bulletSpeed;
        this.angle = angle;
        this.madeInBullet = madeInBullet;
    }

    private void MoveBullet()
    {
        Vector2 position = transformCache.localPosition;
        position += Direction * BulletSpeed * Time.deltaTime;

        transformCache.localPosition = position;
    }

 /*   private void RotateBullet()
    {
        //? 통합하기 확장 메서드
        Direction = Rotate.RotateBullet(Direction, angle);
    } */

    private IEnumerator CreateBullet()
    {
        creating = true; // bullet 에서 bullet을 만드는중

        for (int i = 0; i < 3; i++)
        {
            GameObject bullet = Instantiate(gameObject);
            Transform bulletTransform = bullet.GetComponent<Transform>();
            test_bullet_1 setBullet = bullet.GetComponent<test_bullet_1>();

            bulletTransform.localPosition = transformCache.localPosition;
            bulletTransform.localRotation = Quaternion.identity;
            bulletTransform.localScale = Vector3.one * 0.5f;

            setBullet.SetValue(Direction, 3.0f, Random.Range(0, 360.0f), true);
            yield return new WaitForSeconds(1.0f); // 1초마다 한번씩 호출 x3
        }

        Angle = Random.Range(0, 360);
        Direction = GlobalMethod.Global.RotateDirection(Direction, angle);

        test_manager_1.Instance.OnCollision = false; // 충돌판정을 끝내서 object에서 만들어진 bullet을 움직이게 함.

        yield return new WaitForSeconds(2.0f); 

        creating = false; // bullet 에서 bullet 만들기를 끝냄
        test_manager_1.Instance.WaitTime = false; // bullet 에서 bullet 만들기를 끝냄을 fireobject 에게 보냄.
    }
    
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (gameObject.GetComponent<NoWallDestroy>() != null) // bulletA , bulletB 구분.
        {
            if (coll.gameObject.GetComponent<Wall>() != null) // Wall 에 충돌했을때
            {
                test_manager_1.Instance.OnCollision = true; // 충돌판정을 보냄.
                Destroy(gameObject);
            }
        }
    }
}
