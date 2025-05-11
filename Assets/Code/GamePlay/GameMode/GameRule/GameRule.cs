using System;
using UnityEngine;


public abstract class GameRule
    : Z1Behaviour
{
    private GameMode gameMode;

    protected override void Awake()
    {
        base.Start();
    }
}
