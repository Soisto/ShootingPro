using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : BaseMonoBehaviour
{
    public override void PostInitialize()
    {
        Debug.Log("Test Action PostInitialize");


        KeyInputManager.Instance.BindAction("Shooting", INPUT_EVENT.IE_PRESSED, TestActionFunc);
    }

    private void TestActionFunc()
    {
        Debug.Log("Test Action Func");
    }

}
