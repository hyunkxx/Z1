using System;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using Unity.VisualScripting;


public enum EGameState
{
    None,
    LoadGame,
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
    public Transform PlayerStart => playerStart;

    [SerializeField]
    private GameObject _playerControllerPrefab;

    private EGameState gameState;
    public event Action<EGameState> OnChangeGameState;

    //[SerializeField]
    //GameModeConfig _config;

    //private AsyncOperationHandle<GameObject> _gameRlueHandle;
    //private AsyncOperationHandle<GameObject> _playerControllerHandle;
    

    protected override void OnDestroy()
    {

        base.OnDestroy();
    }

    protected override void Awake()
    {
        base.Awake();

        Debug.Log("GameMode Awake");

        GameManager.Instance.RegisterGameMode(this);
        Rule = GetComponent<GameRule>();

        CreatePlayerController();
    }

    protected override void Start()
    {
        base.Start();

        Debug.Log("GameMode Start");
    }

    private bool CreatePlayerController()
    {
        if (!_playerControllerPrefab)
            return false;

        PlayerController = Instantiate(_playerControllerPrefab).GetComponent<PlayerController>();
        return true;
    }

    public void ChangeGameState(EGameState state)
    {
        if (gameState == state)
            return;

        Debug.Log($"ChangeGameState : {state}");
        gameState = state;
        OnChangeGameState?.Invoke(state);
    }
}
