using UnityEngine;
using System.Collections;

public class PlayerPowerUp : MonoBehaviour
{
    private Transform transformCache;

    public Transform playerTransform;

    public float angle;
 /*   {
        set
        {
            angle = value;
        }
        get
        {
            return angle;
        } 
    } */
    public float radius;

    void Start()
    {
        transformCache = GetComponent<Transform>();

        StartCoroutine(RotatePower());
    }

    void Update()
    {
        if (playerTransform == null)
        {
            playerTransform = GameMgr.Instance.PlayerTransform;
            StartCoroutine(RotatePower());
        }

        if (GameMgr.Instance.PlayerReviving == true)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator RotatePower()
    {
        if (playerTransform == null)
        {
            yield break;
        }

        while (true)
        {
            Vector2 circle = playerTransform.localPosition;

            circle.x = playerTransform.localPosition.x + (Mathf.Sin(angle * Mathf.Deg2Rad) * radius);
            circle.y = playerTransform.localPosition.y + (Mathf.Cos(angle * Mathf.Deg2Rad) * radius);

            transformCache.localPosition = circle;

            angle += 180 * Time.deltaTime;

            yield return null; // 프레임마다 반복
        }
    }
}
