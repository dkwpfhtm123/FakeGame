using UnityEngine;
using System.Collections;

namespace Fake
{
    public class Wall : MonoBehaviour
    {
        void OnCollisionExit2D(Collision2D coll)
        {
            if (coll.gameObject.GetComponent<NoDestroy>() == null)
            {
                Destroy(coll.gameObject);
            }
        }
    }
}
