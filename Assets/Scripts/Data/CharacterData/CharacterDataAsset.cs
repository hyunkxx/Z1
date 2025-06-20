using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharacterDataAsset", menuName = "Scriptable Objects/DataAsset/CharacterDataAsset")]
public class CharacterDataAsset : ScriptableObject
{
    [SerializeField] private int _characterID;
    [SerializeField] private string _name;
    [SerializeField] private string _prefabKey;
    [SerializeField] private int _greenStone;
    [SerializeField] private int _blueStone;
    [SerializeField] private int _redStone;
    [SerializeField] private Sprite _sprite;

    public int CharacterID => _characterID;
    public string Name => _name;
    public string PrefabKey => _prefabKey;
    public int GreenStone => _greenStone;
    public int BlueStone => _blueStone;
    public int RedStone => _redStone;
    public Sprite Sprite => _sprite;
}