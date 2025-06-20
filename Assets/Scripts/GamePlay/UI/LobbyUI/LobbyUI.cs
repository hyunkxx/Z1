using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : UIBase
{
    [SerializeField] CharacterSelector characterSelector;

    private void Awake()
    {
        ClearLobbyDic();
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));
        Bind<TextMeshProUGUI>(typeof(TextMeshPros));
        Bind<Image>(typeof(Images));
    }

    void Start()
    {
        //Main_Panel
        //PanelOpenAction( GetGameObject((int)GameObjects.Main_Panel));
        GetButton((int)Buttons.Lobby_Play_btn).onClick.AddListener(()=> { PanelOpenAction(GetGameObject((int)GameObjects.PlayPanel));  });
        GetButton((int)Buttons.Lobby_Character_btn).onClick.AddListener(() => { PanelOpenAction(GetGameObject((int)GameObjects.CharacterInven_Panel)); });

        //Play_Panel
        GetButton((int)Buttons.Play_DefenseMode_btn).onClick.AddListener(()=> { PanelOpenAction(GetGameObject((int)GameObjects.Defence_ModeSelect_Panel));  });
        GetButton((int)Buttons.Play_SurvivalMode_btn).onClick.AddListener(()=> { PanelOpenAction(GetGameObject((int)GameObjects.Survival_Ready_Panel));  });
        GetButton((int)Buttons.Play_Back_btn).onClick.AddListener(PanelBackAction);

        //Survival_Ready_Panel
        GetButton((int)Buttons.Survival_Ready_CharcterSelect_btn).onClick.AddListener(()=> { PanelOpenAction(GetGameObject((int)GameObjects.CharacterSelect_Panel));  });
        GetButton((int)Buttons.Survival_Ready_Back_btn).onClick.AddListener(PanelBackAction);

        //CharacterInven_Panel
    }
}
