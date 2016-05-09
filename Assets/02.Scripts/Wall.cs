using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.GetComponent<NoWallDestroy>() == null)
        {
            Destroy(coll.gameObject);
        }
    }
}
