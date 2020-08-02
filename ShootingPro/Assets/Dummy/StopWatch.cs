using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWatch : BaseMonoBehaviour
{
    public GameObject obj;

    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    public override void BeginPlay()
    {
        for (int i = 0; i < 10000; ++i)
            CreateObject(obj);
    }

    public override void Tick()
    {
        stopwatch.Start();
    }


    public override void LateTick()
    {
        stopwatch.Stop();
        Debug.Log(stopwatch.ElapsedTicks);
        stopwatch.Reset();
    }
}
