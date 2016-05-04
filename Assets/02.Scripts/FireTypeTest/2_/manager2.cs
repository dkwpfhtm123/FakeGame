using UnityEngine;
using System.Collections;

public class manager2 : MonoBehaviour
{
    private static manager2 instance;
    public static manager2 Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(manager2)) as manager2;

            return instance;
        }
    }

    public bool StopTime;

    void Start()
    {
        StopTime = false;

        StartCoroutine(TimeWatch());
    }

    void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private IEnumerator TimeWatch()
    {
        while (true)
        {
            yield return new WaitForSeconds(3.0f);
            StopTime = true;

            yield return new WaitForSeconds(1.5f);
            StopTime = false;
        }
    }
}
