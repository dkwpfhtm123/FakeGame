using UnityEngine;
using System.Collections;

public class test_script : MonoBehaviour {

    Transform transformCache;
    Transform playerTransform;

    public GameObject PurpleCircle;

	void Start () {
        playerTransform = GameMgr.Instance.PlayerTransform;
        transformCache = GetComponent<Transform>();

        StartCoroutine(CreateB());
	}
	
	void Update () {
        if(playerTransform == null)
        {
            playerTransform = GameMgr.Instance.PlayerTransform;
        }
    }

    IEnumerator CreateB()
    {
        float angle = 0.0f;
        float oneShot = 6.0f;
        float zero = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

            if (playerTransform == null)
            {
                playerTransform = GameMgr.Instance.PlayerTransform;
            }
            for (int i = 0; i < oneShot; i++)
            {
                for (int z = 0; z < oneShot; z++)
                {
                    GameObject bullet = GameObject.Instantiate(PurpleCircle);
                    Transform bulletTransformCache = bullet.transform;
                    test_movescript obj = bullet.GetComponent<test_movescript>();

                    bulletTransformCache.localPosition = transform.localPosition;
                    bulletTransformCache.localRotation = Quaternion.identity;
                    bulletTransformCache.localScale = Vector3.one;

                    obj.BulletSpeed = 3.0f;
                    obj.angle = angle;
                    angle += 60.0f;
                }
            }
            angle = zero;
            zero += 6.0f;
        }
    }
}
