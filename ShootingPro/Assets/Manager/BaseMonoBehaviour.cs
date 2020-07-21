using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonoBehaviour : MonoBehaviour
{
    private int m_iPrimaryNumber = -1;

    public int PrimaryNumber { get => m_iPrimaryNumber; set => m_iPrimaryNumber = value; }

    public virtual void Initialize()
    {
    }

    public virtual void PostInitialize()
    {
    }

    public virtual void BeginPlay()
    {
    }

    public virtual void Tick()
    {
    }

    public virtual void LateTick()
    {
    }

    public virtual void FixedTick()
    {
    }

    protected void Delete()
    {

        Destroy(this, 0f);
    }

    public static Object CreateObject(Object origin)
    {
        BaseMonoBehaviour temp = Instantiate(origin) as BaseMonoBehaviour;
        Core.Instance.AddObject(temp);
        return temp;
    }

    public static Object CreateObject(Object origin, Vector3 position, Quaternion rotation)
    {
        BaseMonoBehaviour temp = Instantiate(origin) as BaseMonoBehaviour;
        Core.Instance.AddObject(temp);
        return Instantiate(origin, position, rotation);
    }

}
