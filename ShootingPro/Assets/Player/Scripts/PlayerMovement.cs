using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : BaseMonoBehaviour
{
    private Vector3 moveVector;

    public override void Initialize()
    {
        KeyInputManager.Instance.BindAxis("MoveFront", MoveFront);
        KeyInputManager.Instance.BindAxis("MoveSide", MoveSide);
    }

    public void MoveFront(float fScale, float fDeltaTime)
    {
        moveVector.y += fScale;
        Debug.Log("MoveFront Callback");
    }

    public void MoveSide(float fScale, float fDeltaTime)
    {
        moveVector.x += fScale;
        Debug.Log("MoveFront Callback");
    }

    public override void Tick() 
    {
    }

    public override void FixedTick()
    {
        transform.position += moveVector;
        moveVector = Vector3.zero;
    }
}
