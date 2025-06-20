using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AIStateSet", menuName = "Scriptable Objects/AIStateSet")]
public class AIStateSet : ScriptableObject
{
    public List<AIStateType> aIStateSet;
}
