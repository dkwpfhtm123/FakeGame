using UnityEngine;
using System.Collections;

public class PlayerPowerUp : MonoBehaviour
{

    Transform playerTransformCache;
    Transform transformCache;
    float radius;
    float angle;

    void Start()
    {
        angle = 0.0f * Mathf.Deg2Rad;
        radius = 1.0f;

        if (GameMgr.Instance.PlayerReviving == true)
        {
            playerTransformCache = GameMgr.Instance.PlayerTransform;
        }

        playerTransformCache = GameMgr.Instance.PlayerTransform;

        StartCoroutine(RotatePower());
        transformCache = GetComponent<Transform>();
    }

    IEnumerator RotatePower()
    {
        yield return new WaitForSeconds(1.0f);

        while (true)
        {
            Vector2 circle = playerTransformCache.localPosition;

            circle.x = playerTransformCache.localPosition.x + (Mathf.Sin(angle * Mathf.Deg2Rad) * radius);
            circle.y = playerTransformCache.localPosition.y + (Mathf.Cos(angle * Mathf.Deg2Rad) * radius);

            transformCache.localPosition = circle;

            angle += 5.0f;
            yield return new WaitForSeconds(0.02f);
        }
    }

    // 180도 x1 / 120도 x2 / 90도 x3 구현예정
}
