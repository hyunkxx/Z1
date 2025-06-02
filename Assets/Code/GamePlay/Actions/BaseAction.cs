using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseAction : Z1Behaviour
{
    public bool TryExecute()
    {
        if (CanExcute())
        {
            ExcuteAction();
            return true;
        }

        return false;
    }

    public abstract void ExcuteAction();
    public abstract bool CanExcute();
}