using System.Collections;
using UnityEngine;

public class SurvivalSpawner : SpawnController
{
    [HideInInspector] 
    public PlayerController playerController;

    [HideInInspector] 
    public GameObject Character;

    private int minXPos = -10, maxXPos = 10;
    private int minYPos = -6, maxYPos = 6;

    private void Start()
    {
        GameMode mode = GameManager.Instance.GameMode;
        mode.OnChangeGameState += OnChangeGameState;
    }

    private void OnDestroy()
    {
        if (GameManager.IsValid())
        {
            GameMode mode = GameManager.Instance.GameMode;
            if (mode)
            {
                mode.OnChangeGameState -= OnChangeGameState;
            }
        }
    }

    private void OnChangeGameState(EGameState state)
    {
        switch(state)
        {
            case EGameState.EnterGame:
                break;
        }
    }

    void Update()
    {

    }


    void AddPool(string _monsterPath, int _size)
    {
        ObjectPool pool = gameObject.AddComponent<ObjectPool>();
        pool.InitializePool(Resources.Load<GameObject>(_monsterPath), _size);
    }

    IEnumerator Spawn(string _type, int _count)
    {
        int curCount = 0;

        while (curCount < _count)
        {
            bool isValidPos = false;
            float xPos = 0, yPos = 0;

            while (!isValidPos)
            {
                xPos = Random.Range(minXPos, maxXPos);
                yPos = Random.Range(minYPos, maxYPos);
                isValidPos = IsValidSpawnPosition(Character.transform.position, new Vector3(xPos, yPos, 0));
            }

            transform.position = Character.transform.position + new Vector3(xPos, yPos, 0);

            GameObject obj = base.Spawn(_type, new Vector3(xPos, yPos));
            curCount++;

            yield return new WaitForSeconds(1f);
        }
    }

    bool IsValidSpawnPosition(Vector3 _targetPos, Vector3 _position)
    {
        if (Vector3.Distance(_position, _targetPos) > 9)
            return true;

        return false;
    }
}
