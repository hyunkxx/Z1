using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{

    protected enum OrderType
    {
        Default,
        Single,
        Multiple,
    }



    static public Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    static Dictionary<Type, UnityEngine.Object[]> _PlayObjects = new Dictionary<Type, UnityEngine.Object[]>();
    static Stack<GameObject> Panel_Order = new Stack<GameObject>();
    Dictionary<GameObject, OrderType> UI_Order = new Dictionary<GameObject, OrderType>();


    protected enum GameObjects
    {
        //Main_Panel,
        PlayPanel,
        Defence_ModeSelect_Panel,
        Defence_Ready_Panel,
        Survival_Ready_Panel,
        CharacterSelect_Panel,
        CharacterSelectContents,
        CharacterInven_Panel,
        ItemInven_Panel,
        SelectItem_Panel,
        //Popup_Panel,
    }

    protected enum Buttons
    {
        //Main_Panel
        Lobby_Play_btn,
        Lobby_Character_btn,

        //Play_Panel
        Play_DefenseMode_btn,
        Play_SurvivalMode_btn,
        Play_Back_btn,

        //Survival_Ready_Panel
        Survival_Ready_CharcterSelect_btn,
        Survival_Ready_Play_btn,
        Survival_Ready_Back_btn,

        //Defence_Ready_Panel
        Defence_Ready_Play_btn,
        Defence_Ready_Back_btn,
        Defence_ModeSelect_Back_btn,

        //CharacterSelect_Panel
        CharacterSelect_btn,
        //CharacterSelect_Back_btn,

        CharacterInven_Close_btn,
        ItemInven_Close_btn,
        ItemInven_Sort_btn,

        //Popup_Panel
        //Popup_Func_btn,
        //Popup_Close_btn,

    }

    protected enum Texts
    {

    }

    protected enum TextMeshPros
    {
        //Popup_Panel
        //Popup_Title_txt,
        //Popup_Des,txt,

    }


    protected enum Images
    {
        Survival_Ready_CharcterSlot_Img,
        CharacterEquip_Character_Img,
    }

    #region UI 맵핑
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);

        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = FindChild(gameObject, names[i], true);
            else
                objects[i] = FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind {names[i]}");
        }
    }

    public static T FindChild<T>(GameObject parents, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (parents == null)
            return null;

        if (!recursive)
        {
            for (int i = 0; i < parents.transform.childCount; i++)
            {
                Transform transform = parents.transform.GetChild(i);

                if (string.IsNullOrEmpty(name) || transform.name == name) // string.IsNullOrEmpty �� ����� name �� Null �̰ų� transform.name�� �̸��� name �� ���ٸ� transform ���� T Ÿ���� �����ɴϴ�.
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in parents.GetComponentsInChildren<T>(true))
            {
                if (string.IsNullOrEmpty(name) || component.name == name) // string.IsNullOrEmpty �� ����� name �� Null �̰ų� ��������� component �� return ���ݴϴ�.
                    return component;
            }
        }

        return null;
    }
    public static GameObject FindChild(GameObject parents, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(parents, name, recursive);

        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static string GetButtonName()
    {
        return EventSystem.current.currentSelectedGameObject.name;
    }


    protected T Get<T>(int _index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        // _objects[_index] 가 가지고 있는 타입은 UnityEngine.Object 이기 문에 T 로 캐스팅을 해줘야합니다.
        return objects[_index] as T;
    }

    protected void ClearLobbyDic()
    {
        _objects.Clear();
        Panel_Order.Clear();
    }
    protected Button GetButton(int _index) { return Get<Button>(_index); }
    protected Text GetText(int _index) { return Get<Text>(_index); }
    protected TextMeshProUGUI GetTextMeshPro(int _index) { return Get<TextMeshProUGUI>(_index); }
    protected Image GetImage(int _index) { return Get<Image>(_index); }
    protected GameObject GetGameObject(int _index) { return Get<GameObject>(_index); }

    #endregion

    protected void PanelAction(GameObject _panel, OrderType orderType = OrderType.Default)
    {
        if (_panel.activeSelf)
        {
            PanelQuitAction(_panel, orderType);
        }
        else
        {
            PanelOpenAction(_panel, orderType);
        }
    }


    protected void PanelOpenAction(GameObject _panel, OrderType orderType = OrderType.Default)
    {
        if (orderType == OrderType.Single)
        {
            CloseAll();
        }

        if (orderType != OrderType.Multiple)
        {

            if (Panel_Order.Count > 0)
                Panel_Order.Peek().SetActive(false);
            Panel_Order.Push(_panel);
        }
        else
        {
            UI_Order.Add(_panel, orderType);
        }

            _panel.SetActive(true);
    }

    protected void PanelQuitAction(GameObject _panel, OrderType orderType = OrderType.Default)
    {
        _panel.SetActive(false);

        if (orderType != OrderType.Multiple)
        {
            Panel_Order.Pop();
        }
        else
        {
            UI_Order.Remove(_panel);
        }
    }

    public void PanelBackAction()
    {
        if (Panel_Order.Count == 0)
            return;


        Panel_Order.Pop().SetActive(false);

        if (Panel_Order.Count > 0)
            Panel_Order.Peek().SetActive(true);


        
        foreach(KeyValuePair<GameObject,OrderType> pair in UI_Order)
        {
            pair.Key.SetActive(false);
         
        }
        UI_Order.Clear();

    }

    public void MoveScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName);
    }

    private void CloseAll()
    {

    }

    //public void ShowPopUp(Action _action, string _setTitleText, string _setDesText)
    //{
    //    GetButton((int)Buttons.Popup_Func_btn).onClick.RemoveAllListeners();
    //    GetButton((int)Buttons.Popup_Close_btn).onClick.RemoveAllListeners();

    //    GetGameObject((int)GameObjects.Popup_Panel).SetActive(true); GetButton((int)Buttons.Popup_Close_btn).gameObject.SetActive(true); GetButton((int)Buttons.Popup_Func_btn).gameObject.SetActive(true);

    //    GetButton((int)Buttons.Popup_Close_btn).onClick.AddListener(() =>
    //    {
    //        GetGameObject((int)GameObjects.Popup_Panel).SetActive(false); GetButton((int)Buttons.Popup_Close_btn).gameObject.SetActive(false); GetButton((int)Buttons.Popup_Func_btn).gameObject.SetActive(false);
    //        //SoundManager.instance.PlayBGM(SoundManager.instance.BGMAudio_0, SoundManager.instance.BackButtonClip);
    //    });
    //    GetTextMeshPro((int)TextMeshPros.Popup_Title_txt).text = _setTitleText;
    //    GetTextMeshPro((int)TextMeshPros.Popup_Des).text = _setDesText;
    //    GetButton((int)Buttons.Popup_Func_btn).onClick.AddListener(() => { GetGameObject((int)GameObjects.Popup_Panel).SetActive(false); GetButton((int)Buttons.Popup_Close_btn).gameObject.SetActive(false); GetButton((int)Buttons.Popup_Func_btn).gameObject.SetActive(false); _action(); });
    //}

    //public void ShowPopUp(Action _action, Action _Adsaction, Button _AdsFuncButton, string _setTitleText, string _setDesText)
    //{
    //    GetButton((int)Buttons.Popup_Func_btn).onClick.RemoveAllListeners();
    //    _AdsFuncButton.onClick.RemoveAllListeners();
    //    GetButton((int)Buttons.Popup_Close_btn).onClick.RemoveAllListeners();

    //    GetGameObject((int)GameObjects.Popup_Panel).SetActive(true); GetButton((int)Buttons.Popup_Close_btn).gameObject.SetActive(true); GetButton((int)Buttons.Popup_Func_btn).gameObject.SetActive(true); _AdsFuncButton.gameObject.SetActive(true);

    //    GetButton((int)Buttons.Popup_Close_btn).onClick.AddListener(() =>
    //    {
    //        GetGameObject((int)GameObjects.Popup_Panel).SetActive(false); GetButton((int)Buttons.Popup_Close_btn).gameObject.SetActive(false); GetButton((int)Buttons.Popup_Func_btn).gameObject.SetActive(false);
    //        //SoundManager.instance.PlayBGM(SoundManager.instance.BGMAudio_0, SoundManager.instance.BackButtonClip);
    //    });
    //    GetTextMeshPro((int)TextMeshPros.Popup_Title_txt).text = _setTitleText;
    //    GetTextMeshPro((int)TextMeshPros.Popup_Des).text = _setDesText;
    //    GetButton((int)Buttons.Popup_Func_btn).onClick.AddListener(() => { GetGameObject((int)GameObjects.Popup_Panel).SetActive(false); GetButton((int)Buttons.Popup_Close_btn).gameObject.SetActive(false); GetButton((int)Buttons.Popup_Func_btn).gameObject.SetActive(false); _AdsFuncButton.gameObject.SetActive(false); _action(); });
    //    _AdsFuncButton.onClick.AddListener(() => { GetGameObject((int)GameObjects.Popup_Panel).SetActive(false); GetButton((int)Buttons.Popup_Close_btn).gameObject.SetActive(false); GetButton((int)Buttons.Popup_Func_btn).gameObject.SetActive(false); _AdsFuncButton.gameObject.SetActive(false); _Adsaction(); });
    //}
}