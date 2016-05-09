using UnityEngine;
using System.Collections;

public class LSBullet : MonoBehaviour
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

    public bool MadeInBullet
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
    private bool creating;

    void Start()
    {
        transformCache = GetComponent<Transform>();
        creating = false;

        Direction = GlobalClass.RotateDirection(Direction, Angle);
    }

    void Update()
    {
        if (LSManager.Instance.OnCollision == false) // 충돌판정이 있기 전까지 움직임.
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
            else // bullet에서 만들어진 bullet 일 때
            {
                MoveBullet();
            }
        }
    }

    //@ 함수이름 바꾸기 전부

    public void SetVariable(Vector2 direction, float bulletSpeed, float angle, bool madeInBullet)
    {
        Direction = direction;
        BulletSpeed = bulletSpeed;
        Angle = angle;
        MadeInBullet = madeInBullet;
    }

    private void MoveBullet()
    {
        Vector2 position = transformCache.localPosition;
        position += Direction * BulletSpeed * Time.deltaTime;

        transformCache.localPosition = position;
    }

    private IEnumerator CreateBullet()
    {
        creating = true; // bullet 에서 bullet을 만드는중

        for (int i = 0; i < 3; i++)
        {
            GameObject bullet = Instantiate(gameObject);
            Transform bulletTransform = bullet.GetComponent<Transform>();
            LSBullet setBullet = bullet.GetComponent<LSBullet>();

            bulletTransform.localPosition = transformCache.localPosition;
            bulletTransform.localRotation = Quaternion.identity;
            bulletTransform.localScale = Vector3.one * 0.5f;

            setBullet.SetVariable(Direction, 3.0f, Random.Range(0, 360.0f), true);
            yield return new WaitForSeconds(1.0f); // 1초마다 한번씩 호출 x3
        }

        Angle = Random.Range(0, 360);
        Direction = GlobalClass.RotateDirection(Direction, Angle);

        LSManager.Instance.OnCollision = false; // 충돌판정을 끝내서 object에서 만들어진 bullet을 움직이게 함.

        yield return new WaitForSeconds(2.0f);

        creating = false; // bullet 에서 bullet 만들기를 끝냄
        LSManager.Instance.WaitTime = false; // bullet 에서 bullet 만들기를 끝냄을 fireobject 에게 보냄.
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (gameObject.GetComponent<NoWallDestroy>() != null) // bulletA , bulletB 구분.
        {
            if (coll.gameObject.GetComponent<Wall>() != null) // Wall 에 충돌했을때
            {
                LSManager.Instance.OnCollision = true; // 충돌판정을 보냄.
                Destroy(gameObject);
            }
        }
    }
}
