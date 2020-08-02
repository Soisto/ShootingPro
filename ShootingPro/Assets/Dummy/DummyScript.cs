using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : BaseMonoBehaviour
{
    int i = 0;
    public override void Tick()
    {
        ++i;
    }
}
