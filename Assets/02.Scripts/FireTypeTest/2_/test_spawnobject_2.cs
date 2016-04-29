using UnityEngine;
using System.Collections;

public class test_spawnobject_2 : MonoBehaviour
{
    public enum SpawnDirection
    {
        Left,
        Right
    }

    public GameObject FireObject;
    public SpawnDirection SpawnObjectDirection;

    private Transform transformCache;
    private Vector2 direction;

    private bool spawning;

    void Start()
    {
        transformCache = GetComponent<Transform>();
        spawning = false;
    }

    void Update()
    {
        if (test_manager_2.Instance.StopTime == false)
        {
            if (spawning == false)
            {
                StartCoroutine(SpawnObject());
            }
        }
    }

    private IEnumerator SpawnObject()
    {
        spawning = true;
        while (test_manager_2.Instance.StopTime == false)
        {
            GameObject fireObject = Instantiate(FireObject);
            test_fireobject_2 setObject = fireObject.GetComponent<test_fireobject_2>();
            Transform objectTransform = fireObject.transform;

            if (SpawnObjectDirection == SpawnDirection.Left)
            {
                setObject.SetValue(Vector2.left, Random.RandomRange(0.9f, 1.1f), test_fireobject_2.FireType.White);
            }
            else
            {
                setObject.SetValue(Vector2.right, Random.RandomRange(0.9f, 1.1f), test_fireobject_2.FireType.Blue);
            }

            Vector3 random = new Vector3(Random.RandomRange(-0.2f, 0.2f), Random.RandomRange(-0.2f, 0.2f), 0); // 초록색밑줄 질문

            objectTransform.localPosition = transformCache.localPosition + random;
            objectTransform.localRotation = Quaternion.identity;
            objectTransform.localScale = Vector3.one;

            yield return new WaitForSeconds(Random.RandomRange(0.9f, 1.1f));
        }
        spawning = false;
    }
}
