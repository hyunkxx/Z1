using System;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;


public enum EGameState
{
    None,
    Initialize,
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

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Addressables.ReleaseInstance(playerController.Character.gameObject);
    }

    protected override void Awake()
    {
        base.Awake();

        GameManager.Instance.RegisterGameMode(this);
        ChangeGameState(EGameState.Initialize);
    }

    protected override void Start()
    {
        base.Start();
        StartGame();

        Debug.Log("GameStart");
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
        Addressables.InstantiateAsync("Assets/Level/Prefabs/Character/Char_BaekSu.prefab").Completed += (handle) =>
        {
            if(handle.Status == AsyncOperationStatus.Succeeded)
            {
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

    public void StartGame()
    {
        SpawnPlayer(PlayerStart.position);
        ChangeGameState(EGameState.EnterGame);
    }
    public void FinishGame()
    {

    }
    public void RestartGame()
    {

    }
}
