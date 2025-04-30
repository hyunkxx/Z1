using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : UIBase
{
    [SerializeField] CharacterSelecter characterSelecter;

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
        GetButton((int)Buttons.Lobby_Play_btn).onClick.AddListener(()=> { PanelAction(GetGameObject((int)GameObjects.PlayPanel));  });
        GetButton((int)Buttons.DefenseMode).onClick.AddListener(()=> { PanelAction(GetGameObject((int)GameObjects.Survival_Ready_Panel));  });
        GetButton((int)Buttons.Survival_Ready_CharcterSelect_btn).onClick.AddListener(()=> { PanelAction(GetGameObject((int)GameObjects.CharacterSelect_Panel));  });
        GetButton((int)Buttons.Survival_Play_btn).onClick.AddListener(()=> { GameManager.Instance.OpenScene("Woo");  });
    }

    void Update()
    {
        
    }
}
