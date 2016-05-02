using UnityEngine;
using System.Collections;

public class EnemyAttackType : MonoBehaviour
{
    public GameObject RedKnife;
    public GameObject BlueKnife;
    public GameObject PurpleCircle;

    public enum AttackType
    {
        RedAttack,
        BlueAttack,
        PurpleCircle
    }

    private static EnemyAttackType instance;
    public static EnemyAttackType Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(EnemyAttackType)) as EnemyAttackType;

            return instance;
        }
    }

    public void FireConeType(Transform spawnTransform, AttackType attackType , float bulletSpeed)
    {
        float oneshot = 5.0f;
        float angle = 60.0f;
        float anglePlus = angle / (oneshot - 1);
        angle *= 0.5f;

        while (oneshot > 0)
        {
            Vector2 targetDirection = RotateBullet(angle, spawnTransform);
            CreateStraightBullet(targetDirection, spawnTransform, attackType, 0 , 1 , bulletSpeed);

            oneshot--;
            angle -= anglePlus;
        }
    }

    public void FireBoomType(Transform spawnTransform, AttackType attackType, float bulletSpeed)     // 무작위 발사후 ConFireType 형태로 발사.
    {
        float oneshot = 5.0f;

        while (oneshot > 0)
        {
            float angle = Random.Range(0.0f, 360.0f);

            Vector2 targetDirection = RotateBullet(angle, spawnTransform);
            CreateStraightBullet(targetDirection, spawnTransform, attackType , 0 , 1 , bulletSpeed);

            oneshot--;
        }
    }

    public void FireSinType(Transform spawnTransform, AttackType attackType, float bulletSpeed)     // 6방향 sin형태의 탄 6발씩 발사 후 20도 꺾어서 6발발사. x6
    {
        StartCoroutine(SinTypeCoroutine(spawnTransform , attackType , bulletSpeed));
    }

    IEnumerator SinTypeCoroutine(Transform spawnTransform, AttackType attackType, float bulletSpeed)
    {
        float angle = 0.0f;
        float oneShot = 6.0f;
        float anglePlus = 0;

        Vector2 imsi = new Vector2(1, 0);

        while (true)
        {
            for (int i = 0; i < oneShot; i++)
            {
                for (int z = 0; z < oneShot; z++)
                {
                    CreateStraightBullet(imsi, spawnTransform, attackType, angle, 0.5f , bulletSpeed);
                    angle += 60.0f;
                }
                yield return new WaitForSeconds(0.2f);
            }
            anglePlus += 20.0f;
            angle = anglePlus;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private Vector2 SinCurve(float currentTime)
    {
        float dx = 300 * currentTime * Mathf.Deg2Rad;
        float dy = Mathf.Sin(dx);

        Vector2 direction = new Vector2(dx , dy);

        return direction.normalized;
    }

    private Vector2 RotateBullet(float angle, Transform spawnTransform)     // 총알 회전
    {
        Transform playerTransform = GameMgr.Instance.PlayerTransform;
        Vector2 targetDirection = (playerTransform.transform.position - spawnTransform.transform.position).normalized;

   /*     Vector2 targetVector = targetDirection;

        targetDirection.x = targetVector.x * Mathf.Cos(angle) - targetVector.y * Mathf.Sin(angle);
        targetDirection.y = targetVector.x * Mathf.Sin(angle) + targetVector.y * Mathf.Cos(angle);

        return targetDirection; */

        return GlobalClass.RotateDirection(targetDirection, angle);
    }

    private void CreateStraightBullet(Vector2 targetDirection, Transform spawnTransform, AttackType attackType , float angle , float localScale , float bulletSpeed) // 직선 탄환 생성
    {
        GameObject bulletObject = null;
        if (attackType == AttackType.RedAttack)
        {
            bulletObject = Instantiate(RedKnife);
        }
        else if (attackType == AttackType.BlueAttack)
        {
            bulletObject = Instantiate(BlueKnife);
        }
        else if(attackType == AttackType.PurpleCircle)
        {
            bulletObject = Instantiate(PurpleCircle);
        }
            
        Transform bulletTransform = bulletObject.transform;
        EnemyBullet bullet = bulletObject.GetComponent<EnemyBullet>();

        bulletTransform.localPosition = spawnTransform.localPosition;
        bulletTransform.localRotation = Quaternion.identity;
        bulletTransform.localScale = Vector3.one * localScale;

        bullet.setValue(targetDirection.normalized, bulletSpeed, angle);

        if (attackType == AttackType.PurpleCircle)
        {
            bullet.MoveType = SinCurve;
        }
        else
        {
            bullet.MoveType = null;
        }
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
