using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;


public sealed class AppManager : Singleton<AppManager>
{
    protected override void Awake()
    {
        base.Awake();

        Database.Instance.Initialize();
        //AssetLoader.Initialize();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        //AssetLoader.UnInitialize();
    }
}
