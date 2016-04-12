using UnityEngine;
using System.Collections;

public class EnemyAttackType : MonoBehaviour
{
    public GameObject RedKnife;
    public GameObject BlueKnife;

    public float BulletSpeed;

    public enum AttackType
    {
        RedAttack,
        BlueAttack
    }

    public static EnemyAttackType Instance = null;

    void Awake()
    {
        Instance = this;
    }

    public void ConFireType(Transform spawnTransform, AttackType attackType)
    {
        float oneshot = 5.0f;
        float angle = 60.0f * Mathf.Deg2Rad;
        float anglePlus = angle / (oneshot - 1);
        angle *= 0.5f;
        BulletSpeed = 2.0f;

        while (oneshot > 0)
        {
            Vector2 targetDirection = BulletTurn(angle, spawnTransform);
            CreateBullet(targetDirection, spawnTransform, attackType);

            oneshot--;
            angle -= anglePlus;
        }
    }

    // 무작위 발사후 ConFireType 형태로 발사.
    public void BoomFireType(Transform spawnTransform, AttackType attackType)
    {
        float oneshot = 5.0f;

        while (oneshot > 0)
        {
            float angle = Random.Range(0.0f, 360.0f) * Mathf.Deg2Rad;
            BulletSpeed = Random.Range(1.0f, 2.0f);

            Vector2 targetDirection = BulletTurn(angle, spawnTransform);
            CreateBullet(targetDirection, spawnTransform, attackType);

            oneshot--;
        }
    }

    // sin 그래프를 따라서 나가는 탄 구현중. 지금은 실행하면 유니티가 멈춤.
    public void SinFireType(Transform spawnTransform, AttackType enemyType)
    {
        float oneshot = 5.0f;
        while (oneshot > 0)
        {
            SinType(spawnTransform);
            oneshot--;
        }
    }

    private void SinType(Transform spawnTransform)
    {
        float dx = 0.0f;
        while (dx < 360.0f)
        {
            dx = 0.0f * Mathf.Deg2Rad;
            float dy = Mathf.Sin(dx);

            GameObject bullet = GameObject.Instantiate(RedKnife);
            Transform bulletTransformCache = bullet.transform;
            EnemyBullet obj = bullet.GetComponent<EnemyBullet>();

            bulletTransformCache.localPosition = spawnTransform.localPosition;
            bulletTransformCache.localRotation = Quaternion.identity;
            bulletTransformCache.localScale = Vector3.one;

            Vector2 direction = new Vector2(dx, dy);

            obj.Direction = direction;
            obj.BulletSpeed = 3.0f;
            obj.MoveBullet(Time.deltaTime);

            dx += 0.1f;
        }
    }

    // 총알 회전
    private Vector2 BulletTurn(float angle, Transform spawnTransform)
    {
        Transform playerTransform = GameMgr.Instance.PlayerTransform;
        Vector2 targetDirection = (playerTransform.transform.position - spawnTransform.transform.position).normalized;

        Vector2 targetVector = targetDirection;

        targetDirection.x = targetVector.x * Mathf.Cos(angle) - targetVector.y * Mathf.Sin(angle);
        targetDirection.y = targetVector.x * Mathf.Sin(angle) + targetVector.y * Mathf.Cos(angle);

        return targetDirection;
    }

    private void CreateBullet(Vector2 targetDirection, Transform spawnTransform, AttackType enemyType)
    {
        GameObject Bullet = null;
        if (enemyType == AttackType.RedAttack)
        {
            Bullet = GameObject.Instantiate(RedKnife);
        }
        else if (enemyType == AttackType.BlueAttack)
        {
            Bullet = GameObject.Instantiate(BlueKnife);
        }

        //@ 변수 이름
        Transform bulletTransformCache = Bullet.transform;
        EnemyBullet bulletObject = Bullet.GetComponent<EnemyBullet>();

        bulletTransformCache.localPosition = spawnTransform.localPosition;
        bulletTransformCache.localRotation = Quaternion.identity;
        bulletTransformCache.localScale = Vector3.one;

        bulletObject.Direction = targetDirection.normalized;
        bulletObject.BulletSpeed = BulletSpeed;
        bulletObject.MoveBullet(Time.deltaTime);
    }

    // Create/Delete/Destroy
    // Initialize/Finalize/Terminate
    // Open/Close
    // Add, Insert/Remove/Clear, Append
    // Set/Get
    // Reset
    // Execute/Run/Launch
    // Move
}
