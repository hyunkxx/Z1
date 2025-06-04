using UnityEngine;


[CreateAssetMenu(fileName = "Condition_HasNearTarget", menuName = "Scriptable Objects/Conditions/Condition_HasNearTarget")]
public class Condition_HasNearTarget : Condition
{
    protected override bool InternalCheck(ICharacterQueryable InQueryable)
    {
        Character character = InQueryable.GetCharacter();
        if (!character)
            return false;

        TargetingComponent targeting = character.TargetingComponent;
        if (!targeting)
            return false;

        return targeting.HasNearTarget();
    }
}
