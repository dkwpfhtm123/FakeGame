using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{
    public Vector2 Direction;
    public float BulletSpeed;
    public float angle;

    public bool StraightBullet = false;
    public bool CurveBullet = false;

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
        if (StraightBullet == true)
        {
            MoveStraightBullet();
        }
        else if (CurveBullet == true)
        {
            MoveCurveBullet();
        }

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

    public void MoveStraightBullet()
    {
        if (transformCache == null)
        {
            transformCache = GetComponent<Transform>();
        }

        Vector2 bulletPosition = transformCache.localPosition;

        bulletPosition.x += Direction.x * BulletSpeed * Time.deltaTime;
        bulletPosition.y += Direction.y * BulletSpeed * Time.deltaTime;

        transformCache.localPosition = bulletPosition;
    }

    private void MoveCurveBullet()
    {
        float dx = 300 * currentTime * Mathf.Deg2Rad;
        float dy = Mathf.Sin(dx);

        Vector2 direction = new Vector2(dx, dy);

        Vector2 targetDirection = direction;

        float Angle = angle * Mathf.Deg2Rad;

        direction.x = targetDirection.x * Mathf.Cos(Angle) - targetDirection.y * Mathf.Sin(Angle);
        direction.y = targetDirection.x * Mathf.Sin(Angle) + targetDirection.y * Mathf.Cos(Angle);

        BulletSpeed = 5.0f;

        transform.localPosition = startingPosition + (BulletSpeed * direction.normalized * currentTime);

        currentTime += Time.deltaTime;
    }

    private IEnumerator BoomBullet()
    {
        yield return new WaitForSeconds(1.0f); // 1 초후 폭발.
        EnemyAttackType.Instance.FireConeType(transformCache, EnemyAttackType.AttackType.RedAttack);
        Destroy(gameObject);
    }
}