using UnityEngine;
using UnityEngine.UI;

public class CharacterInven : UIBase
{
    //int CurSelectIndex = 0;
    [SerializeField] private GameObject CharacterSelecter;
    [SerializeField] private Button[] OpenButtons;
    [SerializeField] private Button[] CloseButtons;

    private void Awake()
    {
        for (int i = 0; i < OpenButtons.Length; i++)
        {
            OpenButtons[i].onClick.AddListener(Link_On);
        }

        for (int i = 0; i < CloseButtons.Length; i++)
        {
            CloseButtons[i].onClick.AddListener(Link_Off);
        }

        //GetButton((int)Buttons.CharacterInven_Back_btn).onClick.AddListener(PanelBackAction);
    }
    void Start()
    {
        Link_On();
    }


    private void Link_On()
    {
        Vector2 Minvec2 = new Vector2(0.5f, 0);
        Vector2 Maxvec2 = new Vector2(1, 1);
        Vector2 DeltaVec2 = new Vector2(0, 0);
        PanelOpenAction(CharacterSelecter,OrderType.Multiple);

        CharacterSelecter.GetComponent<RectTransform>().anchorMin = Minvec2;
        CharacterSelecter.GetComponent<RectTransform>().anchorMax = Maxvec2;
        CharacterSelecter.GetComponent<RectTransform>().sizeDelta = DeltaVec2;
        
        CharacterSelecter.GetComponent<CharacterSelecter>().ActiveButton(false);
        CharacterSelecter.transform.SetAsLastSibling();
    }

    private void Link_Off()
    {
        Vector2 Minvec2 = new Vector2(0f, 0);
        Vector2 Maxvec2 = new Vector2(1, 1);
        Vector2 DeltaVec2 = new Vector2(0, 0);

        CharacterSelecter.GetComponent<RectTransform>().anchorMin = Minvec2;
        CharacterSelecter.GetComponent<RectTransform>().anchorMax = Maxvec2;
        CharacterSelecter.GetComponent<RectTransform>().sizeDelta = DeltaVec2;

        CharacterSelecter.GetComponent<CharacterSelecter>().ActiveButton(true);
    }
    
}
