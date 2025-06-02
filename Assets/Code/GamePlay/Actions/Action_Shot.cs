using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class Action_Shot : Action_Attack
{
    public override void ExcuteAction()
    {
        enabled = true;

        if (EffectPrefab == null)
            return;

        Character character = GetComponent<Character>();
        if (character == null)
            return;

        Transform weaponPivot = character.CharacterView._weaponSocket;
        Transform weaponMuzzle = character.CharacterView._weaponEndSocket;

        GameObject obj = Instantiate(EffectPrefab);
        Effect2D instance = obj.GetComponent<Effect2D>();

        obj.GetComponent<Projectile2D>().Initialize(weaponPivot.right);

        obj.SetActive(true);
        instance.ActivateEffect(gameObject, weaponMuzzle);
    }
}
