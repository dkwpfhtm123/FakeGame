using UnityEngine;
using System.Collections;

public class PlayerPowerUp : MonoBehaviour
{
    private Transform transformCache;

    public float angle;
    public float radius;

    void Update()
    {
        if (GameMgr.Instance.RespawnPlayer == true)
        {
            Destroy(gameObject);
        }
    }

    public void StartRotatePower(Transform playerTransform)
    {
        transformCache = GetComponent<Transform>();
        StartCoroutine(RotatePower(playerTransform));
    }

    private IEnumerator RotatePower(Transform playerTransform)
    {
        while (GameMgr.Instance.RespawnPlayer == false)
        {
            Vector2 circle = playerTransform.localPosition;

            circle.x = playerTransform.localPosition.x + (Mathf.Sin(angle * Mathf.Deg2Rad) * radius);
            circle.y = playerTransform.localPosition.y + (Mathf.Cos(angle * Mathf.Deg2Rad) * radius);

            transformCache.localPosition = circle;

            angle += 180 * Time.deltaTime;
            if(angle > 360.0f)
            {
                angle = 0;
            }

            yield return null; // 프레임마다 반복
        }
    }
}
