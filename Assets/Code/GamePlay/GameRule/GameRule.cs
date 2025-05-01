using System;
using UnityEngine;


public abstract class GameRule
    : Z1Behaviour
{
    private GameMode gameMode;

    protected override void Awake()
    {
        base.Start();
        gameMode = gameObject.GetComponent<GameMode>();
    }

    public void SpawnPlayer(Vector3 location)
    {
        GameObject spawned = Instantiate(GameManager.Instance.tempPlayerPrefab, location, Quaternion.identity);
        Character character = spawned.GetComponent<Character>();

        CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
        cameraMovement.SetViewTarget(character.gameObject);

        PlayerController playerController = gameMode.playerController;
        playerController.BindCharacter(character);
    }
}
