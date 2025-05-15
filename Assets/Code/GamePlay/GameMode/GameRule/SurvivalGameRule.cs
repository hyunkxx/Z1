using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class SurvivalGameRule
    : GameRule
{
    public int CharacterID { get; private set; }

    private AsyncOperationHandle<GameObject> _characterHandle;

    protected override void OnDestroy()
    {
        if (_characterHandle.IsValid())
        {
            Addressables.ReleaseInstance(_characterHandle);
        }

        if (GameManager.IsValid())
        {
            GameManager.Instance.RegisterGameMode(null);
        }

        base.OnDestroy();
    }

    protected override void Awake()
    {
        base.Awake();

        GameManager Game = GameManager.Instance;
        if(!Game.HasPayload())
        {
            Debug.LogWarning("need character id");
            Destroy(gameObject);
            return;
        }

        CharacterID = int.Parse(Game.Payload);

        Transform playerStart = _gameMode.PlayerStart;
        SpawnCharacter(playerStart.position);
    }

    protected override void Start()
    {
        base.Start();
        Debug.Log("Rule Start");
    }

    private bool SpawnCharacter(Vector3 location)
    {
        var AssetData = Database.Instance.CharacterAssetData.GetValueOrDefault(CharacterID);
        if (!AssetData)
            return false;

        Addressables.InstantiateAsync(AssetData.PrefabKey, location, Quaternion.identity).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _characterHandle = handle;

                Character character = handle.Result.GetComponent<Character>();
                CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
                cameraMovement.SetViewTarget(character.gameObject);

                _gameMode.PlayerController.ConnectCharacter(character);
                _gameMode.ChangeGameState(EGameState.ReadyGame);
            }
        };

        return true;
    }
}
