using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{
    public Vector2 Direction;
    public float BulletSpeed;
    public float angle;

    public bool Curve;

    private Transform transformCache;
    private bool firstFrame;
    private BulletTypeScript bulletType;
    private float currentTime;
    private Vector2 startingPosition;

    void Start()
    {
        firstFrame = true;

        transformCache = GetComponent<Transform>();
        bulletType = gameObject.GetComponent<BulletTypeScript>();
        currentTime = 0;
        startingPosition = transformCache.localPosition;

        float angle = GetRotation();
        transform.localRotation = Quaternion.Euler(0, 0, angle);

        if (bulletType.BulletTypeCheck == BulletType.BlueKnife)
        {
            StartCoroutine(BoomBullet());
        }
    }

    void Update()
    {
        MoveCurveBullet();

        if (GameMgr.Instance.OnGoingBoom == true)
        {
            ItemSpawn.Instance.SpawnItem(gameObject.transform, ItemSpawn.ItemTypeObject.ScoreItem);
            Destroy(gameObject);
        }
    }

    private float GetRotation()
    {
        float angle = -Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;
        return angle;
    }

    Vector2 SinCurve()
    {
        float dx = 300 * currentTime * Mathf.Deg2Rad;
        float dy = Mathf.Sin(dx);

        Vector2 direction = new Vector2(dx, dy);

        BulletSpeed = 4.0f; // 여기에 선언해야 안느려지는 이유?

        return direction.normalized;
    }

    private void MoveCurveBullet()
    {
        if (Curve == true)
        {
            Direction = SinCurve();
        }

        Vector2 targetDirection = Direction;

        float Angle = angle * Mathf.Deg2Rad;

        Direction.x = targetDirection.x * Mathf.Cos(Angle) - targetDirection.y * Mathf.Sin(Angle);
        Direction.y = targetDirection.x * Mathf.Sin(Angle) + targetDirection.y * Mathf.Cos(Angle);

        transform.localPosition = startingPosition + (BulletSpeed * Direction * currentTime);

        currentTime += Time.deltaTime;
    }

    private IEnumerator BoomBullet()
    {
        yield return new WaitForSeconds(1.0f); // 1 초후 폭발.
        EnemyAttackType.Instance.FireConeType(transformCache, EnemyAttackType.AttackType.RedAttack);
        Destroy(gameObject);
    }
}