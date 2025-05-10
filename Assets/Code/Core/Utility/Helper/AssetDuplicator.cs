using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


public class AssetDuplicator : MonoBehaviour
{
    private static AssetDuplicator instance;

    public static void CopyFile(string sourcePath, string targetPath, Action<bool> onComplete = null)
    {
        if (instance == null)
        {
            GameObject obj = new GameObject("AssetDuplicator");
            instance = obj.AddComponent<AssetDuplicator>();
        }

        string targetDir = Path.GetDirectoryName(targetPath);
        if (!Directory.Exists(targetDir))
        {
            Directory.CreateDirectory(targetDir);
        }

#if UNITY_EDITOR
        File.Copy(sourcePath, targetPath, true);
        Debug.Log("[AssetDuplicator]  File copy successful: " + targetPath);

        onComplete?.Invoke(true);

        Destroy(instance.gameObject);
        instance = null;
#elif UNITY_ANDROID
        instance.StartCoroutine(CoroutineCopyFile(sourcePath, targetPath));
#endif
    }

    private static IEnumerator CoroutineCopyFile(string sourcePath, string targetPath, Action<bool> onComplete)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(sourcePath))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"[AssetDuplicator] Failed to copy file from StreamingAssets: {request.error}");
                onComplete?.Invoke(false);
                yield break;
            }

            try
            {
                File.WriteAllBytes(targetPath, request.downloadHandler.data);
                Debug.Log($"[AssetDuplicator] File copied to: {targetPath}");
                onComplete?.Invoke(true);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[AssetDuplicator] Error writing file: {ex.Message}");
                onComplete?.Invoke(false);
            }

            Destroy(instance.gameObject);
            instance = null;
        }
    }
}
