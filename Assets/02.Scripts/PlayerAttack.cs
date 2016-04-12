using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{

    public GameObject PlayerAttack1;
    public GameObject PlayerAttack2;
    public Transform FirePosition;  // 발사위치

    private float playerAttackTime = 0.0f;
    private bool shiftCheck = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            shiftCheck = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            shiftCheck = false;
        }

        if (Input.GetMouseButton(0))
        {
            playerAttackTime -= Time.deltaTime;
            if (playerAttackTime < 0.0f)
            {
                playerAttackTime = 0.1f;
                Fire();
            }
        }
    }

    // 발사

    private void Fire()
    {
        GameObject playerBullet = null;

        if (shiftCheck == false)
        {
            playerBullet = Instantiate(PlayerAttack1);
        }

        else
        {
            playerBullet = Instantiate(PlayerAttack2);
        }

        PlayerBullet bulletObject = playerBullet.GetComponent<PlayerBullet>();

        playerBullet.transform.localPosition = FirePosition.localPosition;
        playerBullet.transform.localRotation = Quaternion.identity;
        bulletObject.Speed = 5.0f;
        bulletObject.Damage = 10.0f;
    }
}
