using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEditor.FilePathAttribute;

public static class AssetLoader
{
    private static Dictionary<string, AsyncOperationHandle> resourceHandles = new();

    static AssetLoader()
    {
    }

    public static void Initialize()
    {
        Addressables.InitializeAsync().Completed += InternalInitializeCallback;
    }

    public static void UnInitialize()
    {
        foreach(var entry in resourceHandles)
        {
            if (entry.Value.IsValid())
            {
                entry.Value.Release();
            }
        }
    }

    public static T GetHandleInstance<T>(string key) where T : UnityEngine.Object
    {
        return resourceHandles[key].IsValid() ? (T)resourceHandles[key].Result as T : null;
    }

    private static void InternalInitializeCallback(AsyncOperationHandle<IResourceLocator> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
            Logger.Log("Addressables initialization successful.", ELogLevel.Log);
        else
            Logger.Log("Addressables initialization failed.", ELogLevel.Warning);
    }

    public static void InstantiateAsync<T>(string addressablesPath)
    {
        Addressables.InstantiateAsync(addressablesPath, Vector3.zero, Quaternion.identity).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                AddHandle(addressablesPath, handle);
            }
        };
    }

    public static void LoadAssetAsync<T>(string addressablesPath, Action _loadedComplete)
    {
        if (resourceHandles.ContainsKey(addressablesPath)) { _loadedComplete?.Invoke(); return; }

        Addressables.LoadAssetAsync<T>(addressablesPath).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                AddHandle(addressablesPath, handle);
                _loadedComplete?.Invoke();
            }
        };
    }

    public static void AddHandle(string addressablesPath, AsyncOperationHandle handle)
    {
        if (!resourceHandles.ContainsKey(addressablesPath))
            resourceHandles.Add(addressablesPath, handle);
    }
}