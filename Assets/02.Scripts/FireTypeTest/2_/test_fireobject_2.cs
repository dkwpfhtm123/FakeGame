using UnityEngine;
using System.Collections;

public class test_fireobject_2 : MonoBehaviour
{
    public GameObject WhiteBullet;
    public GameObject BlueBullet;

    public enum FireType
    {
        White,
        Blue
    }

    private Vector2 direction;
    public Vector2 Direction
    {
        get
        {
            return direction;
        }
        private set
        {
            direction = value;
        }
    }

    private float objectSpeed;
    public float ObjectSpeed
    {
        get
        {
            return objectSpeed;
        }
        private set
        {
            objectSpeed = value;
        }
    }

    private bool firing;
    private FireType type;

    private Transform transformCache;

    void Start()
    {
        transformCache = GetComponent<Transform>();
        firing = false;
    }

    void Update()
    {
        if (test_manager_2.Instance.StopTime == false)
        {
            MoveObject();
            if (firing == false)
            {
                StartCoroutine(FireBullet());
            }
        }
    }

    public void SetValue(Vector2 direction, float objectSpeed, FireType type)
    {
        this.direction = direction;
        this.objectSpeed = objectSpeed;
        this.type = type;
    }

    private void MoveObject()
    {
        Vector2 position = transformCache.localPosition;
        position += direction * objectSpeed * Time.deltaTime;
        transform.localPosition = position;
    }

    private IEnumerator FireBullet()
    {
        firing = true;
        bool Up = true;

        for (int i = 0; i < 2; i++)
        {
            GameObject bullet = null;
            if (type == FireType.White)
            {
                bullet = Instantiate(WhiteBullet);
            }
            else if (type == FireType.Blue)
            {
                bullet = Instantiate(BlueBullet);
            }
            test_bullet_2 setBullet = bullet.GetComponent<test_bullet_2>();
            Transform bulletTransform = bullet.transform; // 질문 Transform / Getcomponent<transform>

            bulletTransform.SetParent(transformCache, false); // bulletTransform.parent == bulletTransform.SetParent(transformCache , true);  // 두번째 인자가 true면 시작위치 = 부모위치 , false면 시작위치 = null
            bulletTransform.localPosition = Vector3.zero;
            bulletTransform.localRotation = Quaternion.identity;
            bulletTransform.localScale = Vector2.one;

            if (Up == true)
            {
                setBullet.SetValue(GlobalMethod.Global.RotateDirection(direction, 90.0f), 1.0f);
            }
            else
            {
                setBullet.SetValue(GlobalMethod.Global.RotateDirection(direction, -90.0f), 1.0f);
            }

            Up = false;
        }

        yield return new WaitForSeconds(0.2f);
        firing = false;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.GetComponent<Wall>() != null)
        {
            transformCache.DetachChildren();
            Destroy(gameObject);
        }
    }
}
