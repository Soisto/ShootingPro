using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject obj;

    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    private void Start()
    {
        for (int i = 0; i < 10000; ++i)
            Instantiate(obj);
    }

    void Update()
    {
        stopwatch.Start();
    }

    private void LateUpdate()
    {
        stopwatch.Stop();
        Debug.Log(stopwatch.ElapsedTicks);
        stopwatch.Reset();
    }
}
