using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{
    public delegate Vector2 MoveTypeDelegate(float deltaTime);
    public MoveTypeDelegate MoveType;

    public Vector2 Direction;
    public float BulletSpeed;
    public float Angle;

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

    private void MoveCurveBullet()
    {
        if (MoveType != null)
        {
            Direction = MoveType(currentTime);
        }
        Vector2 targetDirection = Direction;

        float radAngle = Angle * Mathf.Deg2Rad;

        Direction.x = targetDirection.x * Mathf.Cos(radAngle) - targetDirection.y * Mathf.Sin(radAngle);
        Direction.y = targetDirection.x * Mathf.Sin(radAngle) + targetDirection.y * Mathf.Cos(radAngle);

        transform.localPosition = startingPosition + (BulletSpeed * Direction * currentTime);

        currentTime += Time.deltaTime;
    }

    private IEnumerator BoomBullet()
    {
        yield return new WaitForSeconds(1.0f); // 1 초후 폭발.
        EnemyAttackType.Instance.FireConeType(transformCache, EnemyAttackType.AttackType.RedAttack , 2.0f);
        Destroy(gameObject);
    }
}