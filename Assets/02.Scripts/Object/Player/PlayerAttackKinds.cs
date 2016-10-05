using UnityEngine;
using System.Collections;

namespace Fake
{
    namespace Player
    {
        public class PlayerAttackKinds : MonoBehaviour
        {
            public GameObject PlayerSlowAttack
            {
                get;
                set;
            }
            public GameObject PlayerFastAttack
            {
                get;
                set;
            }

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

            private void Fire()     // 발사
            {
                GameObject playerBullet = null;

                if (shiftCheck == false)
                {
                    playerBullet = Instantiate(PlayerFastAttack);
                }

                else
                {
                    playerBullet = Instantiate(PlayerSlowAttack);
                }

                PlayerBullet bulletObject = playerBullet.GetComponent<PlayerBullet>();

                playerBullet.transform.localPosition = FirePosition.localPosition;
                playerBullet.transform.localRotation = Quaternion.identity;
                bulletObject.Speed = 5.0f;
                bulletObject.Damage = 10.0f;
          //      bulletObject.type = BaseBullet.BulletType.Player;
            }
        }
    }
}