using UnityEngine;
using System.Collections;

public class RainManager : MonoBehaviour
{
    private static RainManager instance;
    public static RainManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<RainManager>();

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
