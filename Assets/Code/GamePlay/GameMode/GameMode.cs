using System;
using UnityEngine;


public enum EGameState
{
    None,
    Ready,
    InProgress,
    Paused,
    Success,
    Failure,
    GameOver,
    Restarting
}

public class GameMode
    : Z1Object
{
    public GameRule gameRule;
    public PlayerController playerController;

    public Transform PlayerStart;

    public EGameState gameState = EGameState.Ready;
    public Action<EGameState> OnChangeGameState;

    protected override void Awake()
    {
        base.Awake();

        GameManager.Instance.RegisterGameMode(this);
        gameState = EGameState.Ready;
    }

    protected override void Start()
    {
        base.Start();
        StartGame();
    }

    public void ChangeGameState(EGameState state)
    {
        if (gameState == state)
            return;

        gameState = state;
        OnChangeGameState?.Invoke(state);
    }

    public void StartGame()
    {
        gameRule.SpawnPlayer(PlayerStart.position);
        ChangeGameState(EGameState.InProgress);
    }
    public void FinishGame()
    {

    }
    public void RestartGame()
    {

    }
}
