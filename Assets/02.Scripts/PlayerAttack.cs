using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

    public GameObject PlayerAttack1;
    public GameObject PlayerAttack2;
    public Transform FirePosition;  // 발사위치

    private float playerAttackTime = 0.0f;
    private bool shiftCheck = false;

    /*    private void Update()  // 시간 텀을 두는 방식 2가지.
        {
            // 1
            if (Input.GetKey(KeyCode.Space))
            {
                if (Time.time - lastShootTime > 0.2f)
                {
                    // Fire();
                    lastShootTime = Time.time;
                }
            }
             
            shootingTime -= Time.deltaTime;  // 2번째
            if (Input.GetKey(KeyCode.Space))
            {
                if (shootingTime < 0.0f)
                {
                    // Fire();
                    shootingTime = 0.2f;
                }
            }
        } */

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

    void Fire()
    {
        if (shiftCheck == false) {
            GameObject playerBullet = Instantiate(PlayerAttack1);
            PlayerBullet obj = playerBullet.GetComponent<PlayerBullet>();

            playerBullet.transform.localPosition = FirePosition.localPosition;
            playerBullet.transform.localRotation = Quaternion.identity;
            obj.Speed = 5.0f;
            obj.Damage = 10.0f;


        }

        else if (shiftCheck == true)
        {
            GameObject playerBullet = Instantiate(PlayerAttack2);
            PlayerBullet obj = playerBullet.GetComponent<PlayerBullet>();

            playerBullet.transform.localPosition = FirePosition.localPosition;
            playerBullet.transform.localRotation = Quaternion.identity;
            obj.Speed = 2.5f;
            obj.Damage = 10.0f;
        }
    }
}
