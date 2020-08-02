using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    private static Core instance = null;

    private Core() { }

    private List<BaseMonoBehaviour> GameObjects = new List<BaseMonoBehaviour>();

    public static Core Instance
    {
        get
        {
            return instance;
        }
    }

    
    public void AddObject(BaseMonoBehaviour _GameObject)
    {
        if (null != _GameObject)
        {
            GameObjects.Add(_GameObject);
        }
    }

    public void DeleteObject(BaseMonoBehaviour _GameObject)
    {
        int idx = _GameObject.PrimaryNumber;
        GameObjects.RemoveAt(idx);

        for(int i = 0; i < GameObjects.Count; ++i)
        {
            GameObjects[i].PrimaryNumber += 1;
        }

        // 이벤트 매니저에서 삭제관련된 이벤트를 만들어야 할듯
        //GameObjects.Remove(_GameObject);  
    }

    //public Object CreateObject(Object origin)
    //{
    //    BaseMonoBehaviour temp = Instantiate(origin) as BaseMonoBehaviour;
    //    AddObject(temp);
    //    return temp;
    //}

    //public Object CreateObject(Object origin, Vector3 position, Quaternion rotation)
    //{
    //    BaseMonoBehaviour temp = Instantiate(origin) as BaseMonoBehaviour;
    //    AddObject(temp);
    //    return Instantiate(origin, position , rotation);
    //}

    private void Awake()
    {
        if(instance == null)
            instance = this;

        KeyInputManager.Instance.Init();
        KeyInputManager.Instance.AddAxisKey("MoveFront", 'w', 1f, 0);
        KeyInputManager.Instance.AddAxisKey("MoveFront", 's', -1f, 0);
        KeyInputManager.Instance.AddAxisKey("MoveSide", 'd', 1f, 0);
        KeyInputManager.Instance.AddAxisKey("MoveSide", 'a', -1f, 0);

        KeyInputManager.Instance.AddActionKey("Shooting", 'p', 0);



        GameObjects.AddRange(FindObjectsOfType<BaseMonoBehaviour>());

        for (int i = 0; i < GameObjects.Count; ++i)
        {
            GameObjects[i].Initialize();
        }

    }

    private void OnEnable()
    {
        for (int i = 0; i < GameObjects.Count; ++i)
        {
            GameObjects[i].PostInitialize();
        }
    }

    void Start()
    {
        for (int i = 0; i < GameObjects.Count; ++i)
        {
            if (GameObjects[i].gameObject.activeSelf)
                GameObjects[i].BeginPlay();
        }
    }

    void Update()
    {
        KeyInputManager.Instance.Logic();

        for (int i = 0; i < GameObjects.Count; ++i)
        {
            GameObjects[i].Tick();
        }
    }


    private void LateUpdate()
    {
        for (int i = 0; i < GameObjects.Count; ++i)
        {
            if (GameObjects[i].gameObject.activeSelf)
                GameObjects[i].LateTick();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < GameObjects.Count; ++i)
        {
            if (GameObjects[i].gameObject.activeSelf)
                GameObjects[i].FixedTick();
        }
    }
}