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
        AssetLoader.Initialize();
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))
            AssetLoader.UnInitialize();
    }

    void OnDisable()
    {
        AssetLoader.UnInitialize();
    }
}
