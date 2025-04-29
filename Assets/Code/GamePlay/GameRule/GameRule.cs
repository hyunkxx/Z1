using System;
using UnityEngine;


public abstract class GameRule
    : Z1Object
{
    public EGameState gameState = EGameState.Ready;
    public Action<EGameState> OnChangeGameState;

    private GameMode gameMode;

    public void ChangeGameState(EGameState state)
    {
        if (gameState == state)
            return;

        gameState = state;
        OnChangeGameState?.Invoke(state);
    }

    protected override void Awake()
    {
        base.Start();
        gameMode = gameObject.GetComponent<GameMode>();
    }

    public void SpawnPlayer(Vector3 location)
    {
        GameObject spawned = Instantiate(GameManager.Instance.tempPlayerPrefab, location, Quaternion.identity);
        Character character = spawned.GetComponent<Character>();
        PlayerController playerController = gameMode.playerController;
        playerController.BindCharacter(character);
    }
}
