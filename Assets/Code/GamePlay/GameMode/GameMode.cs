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
    public PlayerController PlayerController { get; private set; }
    public GameRule Rule { get; private set; }

    [SerializeField]
    private Transform playerStart;

    [SerializeField]
    private GameObject _playerControllerPrefab;

    [SerializeField]
    private GameObject _gameRulePrefab;

    private EGameState gameState;
    public event Action<EGameState> OnChangeGameState;

    //[SerializeField]
    //GameModeConfig _config;

    //private AsyncOperationHandle<GameObject> _gameRlueHandle;
    //private AsyncOperationHandle<GameObject> _playerControllerHandle;
    private AsyncOperationHandle<GameObject> _characterHandle;

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if(_characterHandle.IsValid())
        {
            Addressables.ReleaseInstance(_characterHandle);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        GameManager.Instance.RegisterGameMode(this);
        InitializeGame();
    }

    protected override void Start()
    {
        base.Start();

    }

    public bool InitializeGame()
    {
        if (!_gameRulePrefab || !_playerControllerPrefab)
            return false;

        Rule = Instantiate(_gameRulePrefab).GetComponent<GameRule>();
        PlayerController = Instantiate(_playerControllerPrefab).GetComponent<PlayerController>();

        SpawnPlayer(playerStart.position);
        return true;
    }

    public void ChangeGameState(EGameState state)
    {
        Debug.Log("ChangeGameState");
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
                _characterHandle = handle;

                Character character = handle.Result.GetComponent<Character>();
                CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
                cameraMovement.SetViewTarget(character.gameObject);

                PlayerController.ConnectCharacter(character);
            }

            ChangeGameState(EGameState.ReadyGame);
        };
    }

    public void TeleportPlayer(Vector3 location)
    {
        if (!PlayerController.Character)
            return;

        PlayerController.Character.transform.position = location;
    }
}
