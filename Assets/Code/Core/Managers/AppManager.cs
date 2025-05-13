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
    private void Awake()
    {
        //AssetLoader.Initialize();
    }

    void OnDisable()
    {
        //AssetLoader.UnInitialize();
    }
}
