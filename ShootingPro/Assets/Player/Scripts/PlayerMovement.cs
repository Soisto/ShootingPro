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

    public void MoveFront(float fScale)
    {
        moveVector.y += fScale;
    }

    public void MoveSide(float fScale)
    {
        moveVector.x += fScale;
    }

    public override void Tick() 
    {
       
    }

    public override void FixedTick()
    {
        transform.position += moveVector * Time.fixedDeltaTime;
        moveVector = Vector3.zero;
    }
}
