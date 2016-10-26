using UnityEngine;
using System.Collections;

namespace Fake
{
    public class Wall : MonoBehaviour
    {
        void OnCollisionExit2D(Collision2D coll)
        {
            if (coll.gameObject.GetComponent<BaseBullet>() != null)
            {
                if (coll.gameObject.GetComponent<BaseBullet>().DestroyWhenHitWall == true)
                {
                    Destroy(coll.gameObject);
                }
            }
        }
    }
}
