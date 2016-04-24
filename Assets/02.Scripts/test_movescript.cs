using UnityEngine;
using System.Collections;

public class test_movescript : MonoBehaviour
{
    public Vector2 Direction;
    public float BulletSpeed;
    public bool MakingBullet;
    public float Angle;

    private Transform transformCache;
    private Vector2 startingPosition;
    private bool creating;

    void Start()
    {
        transformCache = GetComponent<Transform>();
        startingPosition = transformCache.localPosition;
        creating = false;

        RotateBullet();
    }

    void Update()
    {
        if (test_managerscript.Instance.oncollision == false)
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

        Vector2 bulletPosition = transformCache.localPosition;

        var position = transformCache.localPosition;
        position.x += Direction.x * BulletSpeed * Time.deltaTime;
        position.y += Direction.y * BulletSpeed * Time.deltaTime;

        transformCache.localPosition = position;

        //       transformCache.localPosition = position;
        //       transform.localPosition = startingPosition + (BulletSpeed * Direction.normalized * currentTime);

        //    currentTime += Time.deltaTime;
    }

    void RotateBullet()
    {
        Vector2 targetDirection = Direction;
        float anglePlus = Angle * Mathf.Deg2Rad;

        Direction.x = targetDirection.x * Mathf.Cos(anglePlus) - targetDirection.y * Mathf.Sin(anglePlus);
        Direction.y = targetDirection.x * Mathf.Sin(anglePlus) + targetDirection.y * Mathf.Cos(anglePlus);
    }

    IEnumerator CreateBullet()
    {
        for (int i = 0; i < 3; i++)
        {
            test_managerscript.Instance.waitTime = true;
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

            yield return new WaitForSeconds(1.0f);

            creating = false;
        }
        Angle = Random.Range(0, 360);
        RotateBullet();

        test_managerscript.Instance.oncollision = false;
        yield return new WaitForSeconds(2.0f);
        test_managerscript.Instance.waitTime = false;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (gameObject.GetComponent<NoWallDestroy>())
        {
            if (coll.gameObject.GetComponent<Wall>() == true)
            {
                test_managerscript.Instance.oncollision = true;
                Destroy(gameObject);
            }
        }
    }
}
