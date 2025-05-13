using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    //public ulong SelectMainCharacterID = 1001;
    public GameObject tempPlayerPrefab;

    public GameMode GameMode => gameMode;
    private GameMode gameMode;

    public GameObject tempEnemy;

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            Character character = GameMode.PlayerController.Character;
            tempEnemy.GetComponent<MovementComponent>().MoveToLocation(character.transform.position, OnReched);
        }
    }

    public void OnReched()
    {
        Debug.Log("REA");
    }

    public void OpenScene(string sceneName)
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for (int i = 0; i < sceneCount; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);

            if (name == sceneName)
            {
                StartCoroutine(LoadSceneAsync(sceneName));
            }
        }
    }
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone)
        {
            Debug.Log("LoadScene");
            yield return null;
        }
    }

    public void RegisterGameMode(GameMode mode)
    {
        this.gameMode = mode;
    }
}
