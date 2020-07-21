using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    private EventManager() { }

    private List<BaseMonoBehaviour> GameObjects = new List<BaseMonoBehaviour>();
}
