using System;
using UnityEngine;


public abstract class ItemDataAsset : ScriptableObject
{
    [Header("Item Properties")]
    [SerializeField] protected int _itemID;
    [SerializeField] protected string _name;
    [SerializeField] protected int _stackLimit;
    [SerializeField] protected Sprite _image;

    public EItemCategory Category { get; protected set; }
    public bool IsAllowStack
    {
        get
        {
            return StackLimit > 1;
        }
    }

    public int ID => _itemID;
    public string Name => _name;
    public int StackLimit => _stackLimit;
    public Sprite Image => _image;

    public abstract Type GetItemClass();
}