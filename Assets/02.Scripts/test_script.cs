using UnityEngine;
using System.Collections;

public class test_script : MonoBehaviour
{
    public GameObject BulletA;
    public GameObject BulletB;

    private bool firstFire;
    private bool firing;

    private Transform transformCache;
    private Transform playerTransform;

    void Start()
    {
        transformCache = GetComponent<Transform>();
        firstFire = false;
        firing = false;

        StartCoroutine(FireBullet());
    }

    void Update()
    {
        if (firing == false)
        {
            if (test_managerscript.Instance.WaitTime == false)
            {
                test_managerscript.Instance.WaitTime = true;
                firstFire = false;
                k = 1;
                StartCoroutine(FireBullet());
            }
        }
    }

    private float k = 1; // 임시변수

    IEnumerator FireBullet()
    {
        firing = true;
        float anglePlus = 0;
        playerTransform = GameMgr.Instance.PlayerTransform;
        Vector2 targetVector = (playerTransform.localPosition - transformCache.localPosition).normalized;

        while (test_managerscript.Instance.OnCollision == false)
        {
            yield return new WaitForSeconds(0.1f); // 발사간격
            for (int i = 0; i < 6; i++)
            {
                GameObject bullet;
                if (firstFire == false)
                {
                    bullet = Instantiate(BulletA);
                }
                else
                {
                    bullet = Instantiate(BulletB);
                }
                Transform bulletTransform = bullet.GetComponent<Transform>();
                test_movescript bulletScript = bullet.GetComponent<test_movescript>();

                bulletTransform.localPosition = transformCache.localPosition;
                bulletTransform.localRotation = Quaternion.identity;
                bulletTransform.localScale = Vector3.one * 0.5f;

                bulletScript.Direction = targetVector;
                bulletScript.BulletSpeed = 3.0f;
                bulletScript.Angle = anglePlus;
                bulletScript.MakingBullet = false;

                anglePlus = k * 60.0f;
                k += 1.0f;
                firstFire = true; // 제일 긴쪽먼저
            }
            anglePlus = 0;
        }
        firing = false;
    }
}
