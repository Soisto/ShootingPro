using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum INPUT_EVENT : int
{
    IE_PRESSED,
    IE_UP,
    IE_DOUBLE,
    IE_HOLD
}

public enum SKey : int
{
    SKEY_CTRL,
    SKEY_ALT,
    SKEY_SHIFT,
    SKEY_END
}

public enum SKey_Type : int
{
    ST_CTRL = 0x1,
    ST_ALT = 0x2,
    ST_SHIFT = 0x4
}

public struct KeyInfo
{
    public char cKey;

    public bool[] bSKey;

    public bool bDown;
    public bool bHold;
    public bool bUp;
}

public struct AxisInfo
{
    public KeyInfo tKeyInfo;
    public float fScale;
}

public struct BindAxisInfo
{
    public string strName;
    public List<AxisInfo> keyList;
    public List<AxisFunc> funcList;
}

public struct BindActionInfo
{
    public string strName;
    public List<KeyInfo> keyList;
    public Dictionary<INPUT_EVENT, List<ActionFunc>> funcDic;
}

public struct InputKey
{
    public KeyCode eKey;
    public char cKey;
    public bool bDown;
    public bool bHold;
    public bool bUp;

    public bool bEnable;

    public InputKey(KeyCode _eKey, char _cKey, bool _bDown, bool _bHold, bool _bUp, bool _bEnable)
    {
        eKey = _eKey;
        cKey = _cKey;
        bDown = _bDown;
        bHold = _bHold;
        bUp = _bUp;

        bEnable = _bEnable;
    }
}



public delegate void ActionFunc();
public delegate void AxisFunc(float fScale);

public class KeyInputManager : Singleton<KeyInputManager>
{
    


    //public struct BindingKey
    //{
    //    public KeyCode m_eKey;
    //    //public KeyState m_eKeyState;
    //    public ActionFunc ActionKeyDown;
    //    public ActionFunc ActionKeyHold;
    //    public ActionFunc ActionKeyUp;

    //    public bool bDown;
    //    public bool bHold;
    //    public bool bUp;

    //    public bool bEnable;
    //}

    

    //public struct ActionInfo
    //{
    //    public KeyInfo tKeyInfo;
    //    public INPUT_EVENT eInputEvent;
    //}

    


    public void AddAxisKey(string strName, char cKey, float fScale, int iSKey)
    {
        BindAxisInfo? Info = FindAxis(strName);

        // Axis가 이미 있는지 검사하여 없으면 생성
        if (null == Info)
        {
            BindAxisInfo AxisInfo;
            AxisInfo.strName = strName;
            AxisInfo.keyList = new List<AxisInfo>();
            AxisInfo.funcList = new List<AxisFunc>();
           
            Info = AxisInfo;
            m_AxisInfoList.Add(AxisInfo);
        }

        IEnumerator<AxisInfo> iter = Info.Value.keyList.GetEnumerator();

        // 동일한 키가 있는지 검사
        while (iter.MoveNext())
        {
            if (iter.Current.tKeyInfo.cKey == cKey)
            {
                return;
            }
        }

        AxisInfo tInfo;

        tInfo.tKeyInfo.bDown = false;
        tInfo.tKeyInfo.bHold = false;
        tInfo.tKeyInfo.bUp = false;

        tInfo.tKeyInfo.cKey = cKey;

        tInfo.fScale = fScale;

        tInfo.tKeyInfo.bSKey = new bool[(int)SKey.SKEY_END];

        if ((iSKey & (int)SKey_Type.ST_CTRL) > 0)
            tInfo.tKeyInfo.bSKey[(int)SKey.SKEY_CTRL] = true;

        if ((iSKey & (int)SKey_Type.ST_ALT) > 0)
            tInfo.tKeyInfo.bSKey[(int)SKey.SKEY_ALT] = true;

        if ((iSKey & (int)SKey_Type.ST_SHIFT) > 0)
            tInfo.tKeyInfo.bSKey[(int)SKey.SKEY_SHIFT] = true;

        Info.Value.keyList.Add(tInfo);

        InputKey temp = m_KeyList[cKey];
        temp.bEnable = true;
        m_KeyList[cKey] = temp;
    }

    public void BindAxis(string strName, AxisFunc func)
    {
        BindAxisInfo? Info = FindAxis(strName);

        if (null == Info)
        {
            return;
        }

        BindAxisInfo temp = Info.Value;

        temp.funcList.Add(func);
        Info = temp;
    }

    private BindAxisInfo? FindAxis(string strName)
    {
        for (int i = 0; i < m_AxisInfoList.Count; ++i)
        {
            if (m_AxisInfoList[i].strName.Equals(strName))
                return m_AxisInfoList[i];
        }

        return null;
    }


    public void AddActionKey(string strName, char cKey, int iSKey)
    {
        BindActionInfo? bindActionInfo = FindAction(strName);

        if(null == bindActionInfo)
        {
            BindActionInfo newBindActionInfo;

            newBindActionInfo.strName = strName;
            newBindActionInfo.keyList = new List<KeyInfo>();
            newBindActionInfo.funcDic = new Dictionary<INPUT_EVENT, List<ActionFunc>>();

            for(int i =0; i < 4; ++i)
            {
                newBindActionInfo.funcDic.Add((INPUT_EVENT)i, new List<ActionFunc>());
            }

            bindActionInfo = newBindActionInfo;
            m_ActoinInfoList.Add(newBindActionInfo);
        }

        IEnumerator<KeyInfo> iter = bindActionInfo.Value.keyList.GetEnumerator();

        while(iter.MoveNext())
        {
            if (iter.Current.cKey == cKey)
                return;
        }

        KeyInfo newActionInfo;

        newActionInfo.bDown = false;
        newActionInfo.bHold = false;
        newActionInfo.bUp = false;
        
        newActionInfo.cKey = cKey;

        newActionInfo.bSKey = new bool[(int)SKey.SKEY_END];

        if ((iSKey & (int)SKey_Type.ST_CTRL) > 0)
            newActionInfo.bSKey[(int)SKey.SKEY_CTRL] = true;

        if ((iSKey & (int)SKey_Type.ST_ALT) > 0)
            newActionInfo.bSKey[(int)SKey.SKEY_ALT] = true;

        if ((iSKey & (int)SKey_Type.ST_SHIFT) > 0)
            newActionInfo.bSKey[(int)SKey.SKEY_SHIFT] = true;


        bindActionInfo.Value.keyList.Add(newActionInfo);

        InputKey tempInputKey = m_KeyList[cKey];
        tempInputKey.bEnable = true;
        m_KeyList[cKey] = tempInputKey;
    }

    public void BindAction(string strName, INPUT_EVENT eInputEvent, ActionFunc func)
    {
        BindActionInfo? bindActionInfo = FindAction(strName);

        if (null == bindActionInfo)
            return;

        BindActionInfo tempActionInfo = bindActionInfo.Value;

        Debug.Log(eInputEvent);

        tempActionInfo.funcDic[eInputEvent].Add(func);    
        bindActionInfo = tempActionInfo;
    }

    private BindActionInfo? FindAction(string strName)
    {
        for(int i = 0; i < m_ActoinInfoList.Count; ++i)
        {
            if (m_ActoinInfoList[i].strName.Equals(strName))
                return m_ActoinInfoList[i];
        }

        return null;
    }

    // 멤버변수 목록
    private List<InputKey> m_KeyList;
    private List<BindAxisInfo> m_AxisInfoList;
    private List<BindActionInfo> m_ActoinInfoList;

    private InputKey[] m_SKeyArr;

    private char m_cPrevKey;


    public void Init()
    {
        m_KeyList = new List<InputKey>();
        m_AxisInfoList = new List<BindAxisInfo>();
        m_ActoinInfoList = new List<BindActionInfo>();
        m_SKeyArr = new InputKey[(int)SKey.SKEY_END];

        m_SKeyArr[(int)SKey.SKEY_CTRL].cKey = (char)KeyCode.LeftControl;
        m_SKeyArr[(int)SKey.SKEY_CTRL].eKey = KeyCode.LeftControl;

        m_SKeyArr[(int)SKey.SKEY_SHIFT].cKey = (char)KeyCode.LeftShift;
        m_SKeyArr[(int)SKey.SKEY_SHIFT].eKey = KeyCode.LeftShift;

        m_SKeyArr[(int)SKey.SKEY_ALT].cKey = (char)KeyCode.LeftAlt;
        m_SKeyArr[(int)SKey.SKEY_ALT].eKey = KeyCode.LeftAlt;

        InputKey key;

        key.bDown = false;
        key.bHold = false;
        key.bUp = false;
        key.bEnable = false;

        for (int i = 0; i < 256; ++i)
        {
            key.cKey = (char)i;
            key.eKey = (KeyCode)i;

            m_KeyList.Add(key);
        }

    }

    // SKeyArr 업데이트 구현 필요
    public void Logic()
    {
        InputUpdate();
        AxisInputUpdate();
        ActionInputUpdate();
    }

    public void InputUpdate()
    {
        for (int i = 0; i < m_KeyList.Count; ++i)
        {
            if (!m_KeyList[i].bEnable)
                continue;

            if (Input.GetKey((m_KeyList[i].eKey)))
            {
                InputKey temp = m_KeyList[i];

                if (!temp.bDown && !temp.bHold)
                {
                    temp.bDown = true;
                    temp.bHold = true;
                }
                else if (temp.bDown)
                {
                    temp.bDown = false;
                }

                m_KeyList[i] = temp;
            }
            else
            {
                InputKey temp = m_KeyList[i];

                if (Input.GetKeyUp(temp.eKey))
                {
                    temp.bDown = false;
                    temp.bHold = false;
                    temp.bUp = true;
                }
                else if (temp.bUp)
                {
                    temp.bUp = false;
                }
                m_KeyList[i] = temp;
            }

        }
    }

    public void AxisInputUpdate()
    {
        IEnumerator<BindAxisInfo> iter = m_AxisInfoList.GetEnumerator();

        while (iter.MoveNext())
        {
            IEnumerator<AxisInfo> iterKey = iter.Current.keyList.GetEnumerator();

            while (iterKey.MoveNext())
            {
                AxisInfo tempAxisInfo = iterKey.Current;

                tempAxisInfo.tKeyInfo.bDown = m_KeyList[iterKey.Current.tKeyInfo.cKey].bDown;
                tempAxisInfo.tKeyInfo.bHold = m_KeyList[iterKey.Current.tKeyInfo.cKey].bHold;
                tempAxisInfo.tKeyInfo.bUp = m_KeyList[iterKey.Current.tKeyInfo.cKey].bUp;

                bool bSKey = true;

                for (int i = 0; i < (int)SKey.SKEY_END; ++i)
                {
                    // 해당 SKey를 눌러야 된다는 것이다.
                    if (tempAxisInfo.tKeyInfo.bSKey[i])
                    {
                        // 키를 눌러야 하는데 안눌렀을 경우
                        // bSKey를 false로 만들고 for문을
                        // 종료한다.
                        if (!m_SKeyArr[i].bHold)
                        {
                            bSKey = false;
                            break;
                        }
                    }
                    else
                    {
                        if (m_SKeyArr[i].bHold)
                        {
                            bSKey = false;
                            break;
                        }
                    }
                }

                IEnumerator<AxisFunc> iterFunc = iter.Current.funcList.GetEnumerator();

                if (bSKey && tempAxisInfo.tKeyInfo.bDown)
                {
                    m_cPrevKey = (iterKey).Current.tKeyInfo.cKey;
                }

                if (bSKey && tempAxisInfo.tKeyInfo.bHold)
                {
                    m_cPrevKey = tempAxisInfo.tKeyInfo.cKey;
                    while (iterFunc.MoveNext())
                    {
                        iterFunc.Current(iterKey.Current.fScale);
                    }
                }

                if (bSKey && tempAxisInfo.tKeyInfo.bUp)
                {
                }

            }

        }

    }

    public void ActionInputUpdate()
    {
        IEnumerator <BindActionInfo> iter = m_ActoinInfoList.GetEnumerator();

        while(iter.MoveNext())
        {
            IEnumerator <KeyInfo> iterKey = iter.Current.keyList.GetEnumerator();

            KeyInfo tempActionInfo;

            while(iterKey.MoveNext())
            {
                tempActionInfo = iterKey.Current;

                tempActionInfo.bDown = m_KeyList[iterKey.Current.cKey].bDown;
                tempActionInfo.bHold = m_KeyList[iterKey.Current.cKey].bHold;
                tempActionInfo.bUp = m_KeyList[iterKey.Current.cKey].bUp;

                bool bSKey = true;


                // 이 코드 좀더 스마트하게 짤 수 있지 않을까  bool 변수하나 사용해서
                // break가 아니라 return 이여도 괜찮지 않을까?
                for (int i = 0; i < (int)SKey.SKEY_END; ++i)
                {
                    if (tempActionInfo.bSKey[i])
                    {
                        if (!m_SKeyArr[i].bDown)
                        {
                            bSKey = false;
                            break;
                        }
                    }
                    else
                    {
                        if(m_SKeyArr[i].bDown)
                        {
                            bSKey = false;
                            break;
                        }
                    }
                }

                Dictionary<INPUT_EVENT, List<ActionFunc>> iterFunc = iter.Current.funcDic;

                if (bSKey && tempActionInfo.bDown)
                {
                    IEnumerator<ActionFunc> funcList = iterFunc[INPUT_EVENT.IE_PRESSED].GetEnumerator();

                    while (funcList.MoveNext())
                    {
                        funcList.Current();
                    }
                }

                if (bSKey && tempActionInfo.bDown)
                {
                    IEnumerator<ActionFunc> funcList = iterFunc[INPUT_EVENT.IE_HOLD].GetEnumerator();

                    while (funcList.MoveNext())
                        funcList.Current();
                }

                if (bSKey && tempActionInfo.bDown)
                {
                    IEnumerator<ActionFunc> funcList = iterFunc[INPUT_EVENT.IE_UP].GetEnumerator();

                    while (funcList.MoveNext())
                        funcList.Current();
                }



            }
        }

    }
}