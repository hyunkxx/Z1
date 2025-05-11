using System;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;


public enum EGameState
{
    None,
    ReadyGame,
    EnterGame,
    ExitGame,
    Paused,
    Success,
    Failure,
    Restarting
}

public sealed class GameMode
    : Z1Behaviour
{
    public GameRule gameRule;
    public PlayerController playerController;

    public Transform PlayerStart;

    private EGameState gameState;
    public event Action<EGameState> OnChangeGameState;

    private AsyncOperationHandle<GameObject> characterHandle;

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if(characterHandle.IsValid())
        {
            Addressables.ReleaseInstance(characterHandle);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        GameManager.Instance.RegisterGameMode(this);
        InitializeGame();

        ChangeGameState(EGameState.ReadyGame);
    }

    protected override void Start()
    {
        base.Start();


        Debug.Log("GameStart");
    }

    public void InitializeGame()
    {
        SpawnPlayer(PlayerStart.position);
        ChangeGameState(EGameState.EnterGame);
    }

    public void ChangeGameState(EGameState state)
    {
        if (gameState == state)
            return;

        gameState = state;
        OnChangeGameState?.Invoke(state);
    }

    public void SpawnPlayer(Vector3 location)
    {
        Addressables.InstantiateAsync("Assets/Level/Prefabs/Character/Character_BackSu.prefab", location, Quaternion.identity).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                characterHandle = handle;

                //GameObject spawned = Instantiate(handle.Result, location, Quaternion.identity);
                Character character = handle.Result.GetComponent<Character>();

                CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
                cameraMovement.SetViewTarget(character.gameObject);
                playerController.BindCharacter(character);
            }
        };
    }

    public void TeleportPlayer(Vector3 location)
    {
        if (!playerController.Character)
            return;

        playerController.Character.transform.position = location;
    }
}
