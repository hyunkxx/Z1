using System;
using UnityEngine;


public abstract class GameRule
    : Z1Behaviour
{
    protected GameMode _gameMode;

    protected override void Awake()
    {
        base.Awake();
        _gameMode = GameManager.Instance.GameMode;
    }
}