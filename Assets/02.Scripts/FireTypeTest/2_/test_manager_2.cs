using UnityEngine;
using System.Collections;

public class test_manager_2 : MonoBehaviour
{
    private static test_manager_2 instance;
    public static test_manager_2 Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(test_manager_2)) as test_manager_2;

            return instance;
        }
        private set
        {
            instance = value;
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
