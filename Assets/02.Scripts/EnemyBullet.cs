using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{
    public Vector2 Direction;
    public float BulletSpeed;
    public float Angle;

    private Transform transformCache;
    private Vector2 startingPosition;

    private bool firstFrame;

    private BulletTypeScript bulletType;

    void Start()
    {
        transformCache = GetComponent<Transform>();

        bulletType = gameObject.GetComponent<BulletTypeScript>();

        firstFrame = true;

        float degree = GetRotation();
        transform.localRotation = Quaternion.Euler(0, 0, degree);

        if (bulletType.BulletTypeCheck == BulletTypeScript.BulletType.BlueKnife)
        {
            StartCoroutine(BoomBullet());
        }
    }

    private float GetRotation()
    {
        float degree = -Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;
        return degree;
    }

    void Update()
    {
        if (firstFrame == false)
        {
            MoveBullet(Time.deltaTime);
        }

        if (GameMgr.Instance.BoomActive == true)
        {
            ItemSpawn.Instance.SpawnItem(gameObject.transform, ItemSpawn.ItemType.ScoreItem);
            Destroy(gameObject);
        }

        firstFrame = false;
    }

    public void MoveBullet(float deltaTime)
    {
        if (transformCache == null)
        {
            transformCache = GetComponent<Transform>();
            startingPosition = transformCache.localPosition;
        }

        Vector2 bulletPosition = transformCache.localPosition;

        var position = transformCache.localPosition;
        position.x += Direction.x * BulletSpeed * deltaTime;
        position.y += Direction.y * BulletSpeed * deltaTime;

        transformCache.localPosition = position;
    }

    private IEnumerator BoomBullet()
    {
        yield return new WaitForSeconds(1.0f); // 1 초후 폭발.
        EnemyAttackType.Instance.ConFireType(transformCache, EnemyAttackType.AttackType.RedAttack);
        Destroy(gameObject);
    }
}