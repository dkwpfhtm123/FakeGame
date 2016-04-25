using UnityEngine;
using System.Collections;

public class test_managerscript : MonoBehaviour
{
    public static test_managerscript Instance = null;

    public bool OnCollision;
    public bool WaitTime;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        OnCollision = false;
        WaitTime = false;
    }
}
