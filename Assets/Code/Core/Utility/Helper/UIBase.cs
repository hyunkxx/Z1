using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    static public Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    static Dictionary<Type, UnityEngine.Object[]> _PlayObjects = new Dictionary<Type, UnityEngine.Object[]>();

    protected enum GameObjects
    {
        PlayPanel,
        Survival_Ready_Panel,
        CharacterSelect_Panel,
        CharacterSelectContents,
        CharacterInven_Panel,
    }

    protected enum Buttons
    {
        Lobby_Play_btn,
        DefenseMode,
        Survival_Ready_CharcterSelect_btn,
        CharacterSelect_btn,
        Survival_Play_btn,
    }

    protected enum Texts
    {

    }

    protected enum TextMeshPros
    {

    }

    protected enum Images
    {
        Survival_Ready_CharcterSlot_Img,
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

    protected void ClearLobbyDic() { _objects.Clear(); }
    protected Button GetButton(int _index) { return Get<Button>(_index); }
    protected Text GetText(int _index) { return Get<Text>(_index); }
    protected TextMeshProUGUI GetTextMeshPro(int _index) { return Get<TextMeshProUGUI>(_index); }
    protected Image GetImage(int _index) { return Get<Image>(_index); }
    protected GameObject GetGameObject(int _index) { return Get<GameObject>(_index); }

    #endregion

    public void PanelAction(GameObject _panel)
    {
        if (_panel.activeSelf)
        {
            _panel.SetActive(false);
        }
        else
        {
            _panel.SetActive(true);
        }
    }
}