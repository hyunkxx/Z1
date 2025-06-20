using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


/*
 * CreateJoystick
 * PlayerInputProcess
 */
public class SurvivalController
    : PlayerController
{
    [SerializeField]
    private GameObject Indicator;
    private TargetingComponent targeting;

    AsyncOperationHandle<GameObject> HUD_Handle;

    protected override void OnDestroy()
    {
        if(HUD_Handle.IsValid() && HUD_Handle.Result != null)
        {
            Addressables.ReleaseInstance(HUD_Handle.Result);
        }

        base.OnDestroy();
    }

    public override void ConnectCharacter(Character target)
    {
        base.ConnectCharacter(target);

        Indicator = Instantiate(Indicator);
        targeting = character.GetComponentInChildren<TargetingComponent>();

        //Survival_HUD
        Addressables.InstantiateAsync("Survival_HUD").Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                HUD_Handle = handle;
                HUD = handle.Result.gameObject.GetComponent<HUDBase>();
                HUD.Initialize();
            }
        };
    }

    protected override void Update()
    {
        if (!character)
        {
            Indicator.SetActive(false);
            return;
        }

        base.Update();
        EvaluateAxisKeyState();
        
        if(targeting.HasNearTarget())
        {
            Indicator.SetActive(true);
            GameObject tempGO = targeting.GetNearestTarget();
            if(tempGO)
            {
                Indicator.transform.position = tempGO.transform.position;
            }
        }
        else
        {
            Indicator.SetActive(false);
        }
    }

    protected void EvaluateAxisKeyState()
    {
        if (inputLock)
        {
            character.Movement.MoveToDirection(Vector2.zero);
            return;
        }

        Vector2 inputDirection;
        inputDirection.x = Input.GetAxisRaw("Horizontal");
        inputDirection.y = Input.GetAxisRaw("Vertical");

        character.OnInputAxis(inputDirection);
    }
}
