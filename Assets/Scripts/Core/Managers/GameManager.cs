using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    public string Payload { get; private set; }
    public GameMode GameMode { get; private set; }

    public bool HasPayload()
    {
        return !string.IsNullOrEmpty(Payload);
    }

    public void OpenScene(string sceneName, string payload = "")
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);

            if (name == sceneName)
            {
                this.Payload = payload;
                StartCoroutine(LoadSceneAsync(sceneName));
            }
        }
    }
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        Debug.Log("LoadScene");

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        Debug.Log("Scene Load Complete");
    }

    public void RegisterGameMode(GameMode mode)
    {
        this.GameMode = mode;
    }
}
