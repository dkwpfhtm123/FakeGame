using UnityEngine;
using System.Collections;

public class spawnobject2 : MonoBehaviour
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
        if (manager2.Instance.StopTime == false)
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
        while (manager2.Instance.StopTime == false)
        {
            GameObject fireObject = Instantiate(FireObject);
            fireobject2 setObject = fireObject.GetComponent<fireobject2>();
            Transform objectTransform = fireObject.transform;

            if (SpawnObjectDirection == SpawnDirection.Left)
            {
                setObject.SetValue(Vector2.left, Random.Range(0.9f, 1.1f), fireobject2.FireType.White);
            }
            else
            {
                setObject.SetValue(Vector2.right, Random.Range(0.9f, 1.1f), fireobject2.FireType.Blue);
            }

            Vector3 random = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);

            objectTransform.localPosition = transformCache.localPosition + random;
            objectTransform.localRotation = Quaternion.identity;
            objectTransform.localScale = Vector3.one;

            yield return new WaitForSeconds(Random.Range(0.95f, 1.05f));
        }
        spawning = false;
    }
}
