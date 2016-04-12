using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    /*안 쓰는 함수는 없애기*/
    /*태그는 직접 비교하지 말고 CompareTag*/
    /*태그보다는 대부분의 상황에서는 GetComponent가 맞다. 어떤 스크립트가 있는가를 비교 */

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.GetComponent<NoWallDestroy>() == false)
        {
            Destroy(coll.gameObject);
        }
    }
}
