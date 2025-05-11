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
        foreach(var e in resourceHandles)
        {
            if (e.Value.IsValid())
            {
                e.Value.Release();
            }
        }

        Debug.Log("un");
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

        
        Addressables.LoadAssetAsync<Material>("URP_SpriteDefaultLit").Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                resourceHandles.Add("URP_SpriteDefaultLit", handle);
                Debug.Log("dd");
                //GameObject spawned = Instantiate(handle.Result, location, Quaternion.identity);
                //Character character = handle.Result.GetComponent<Character>();

                //CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
                //cameraMovement.SetViewTarget(character.gameObject);
                //playerController.BindCharacter(character);
            }
        };
    }

    public static void Load<T>()
    {
        Addressables.InstantiateAsync("Assets/Level/Prefabs/Character/Character_BackSu.prefab", Vector3.zero, Quaternion.identity).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                resourceHandles.Add("Assets/Level/Prefabs/Character/Character_BackSu.prefab", handle);

                //GameObject spawned = Instantiate(handle.Result, location, Quaternion.identity);
                //Character character = handle.Result.GetComponent<Character>();

                //CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
                //cameraMovement.SetViewTarget(character.gameObject);
                //playerController.BindCharacter(character);
            }
        };
    }
}
