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

    protected override void Awake()
    {
        base.Awake();

        GameManager.Instance.RegisterGameMode(this);
        gameRule.gameState = EGameState.Ready;
    }

    protected override void Start()
    {
        base.Start();
        StartGame();
    }

    public void ChangeGameState(EGameState state)
    {
        gameRule.ChangeGameState(state);
    }

    public void StartGame()
    {
        gameRule.SpawnPlayer(PlayerStart.position);
        gameRule.ChangeGameState(EGameState.InProgress);
    }
    public void FinishGame()
    {

    }
    public void RestartGame()
    {

    }
}
