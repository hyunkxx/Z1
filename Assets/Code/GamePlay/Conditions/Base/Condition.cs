using UnityEngine;


public abstract class Condition : ScriptableObject
{
    [SerializeField] public bool InverseCondition = false;

    public bool IsReversed() { return InverseCondition; }
    public bool Check(ICharacterQueryable InQueryable) { return InternalCheck(InQueryable) != InverseCondition; }
    protected abstract bool InternalCheck(ICharacterQueryable InQueryable);
}
