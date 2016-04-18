using UnityEngine;
using System.Collections;

public class test_movescript : MonoBehaviour
{
    public Vector2 Direction;
    public float BulletSpeed;

    Transform transformCache;

    float currentTime;
    Vector2 startingPosition;

    bool firstFrame;

    void Start()
    {
        firstFrame = true;
        transformCache = GetComponent<Transform>();
        currentTime = 0;
        startingPosition = transformCache.localPosition;
    }

    public float angle;
    public float dx;
    public float dy;

    void Update()
    {
        if (firstFrame == false)
        {
            dx = 100 * currentTime * Mathf.Deg2Rad;
            dy = Mathf.Sin(dx);

            Vector2 direction = new Vector2(dx, dy);

            Vector2 targetDirection = direction;

            float Angle = angle * Mathf.Deg2Rad;

            direction.x = targetDirection.x * Mathf.Cos(Angle) - targetDirection.y * Mathf.Sin(Angle);
            direction.y = targetDirection.x * Mathf.Sin(Angle) + targetDirection.y * Mathf.Cos(Angle);

            BulletSpeed = 5.0f;

            transform.localPosition = startingPosition + (BulletSpeed * direction.normalized * currentTime);

            currentTime += Time.deltaTime;
        }

        firstFrame = false;
    }
}
