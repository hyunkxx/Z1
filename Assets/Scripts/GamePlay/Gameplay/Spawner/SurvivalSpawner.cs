using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region ORIGINAL_CLASS
//public class SurvivalSpawner : SpawnController
//{
//    [HideInInspector]
//    public PlayerController playerController;

//    [HideInInspector]
//    public GameObject Character;

//    private int minXPos = -10, maxXPos = 10;
//    private int minYPos = -6, maxYPos = 6;

//    private void Start()
//    {
//        GameMode mode = GameManager.Instance.GameMode;
//        mode.OnChangeGameState += OnChangeGameState;
//    }

//    private void OnDestroy()
//    {
//        if (GameManager.IsValid())
//        {
//            GameMode mode = GameManager.Instance.GameMode;
//            if (mode)
//            {
//                mode.OnChangeGameState -= OnChangeGameState;
//            }
//        }
//    }

//    private void OnChangeGameState(EGameState state)
//    {
//        switch (state)
//        {
//            case EGameState.EnterGame:
//                break;
//        }
//    }

//    void Update()
//    {

//    }


//    void AddPool(string _monsterPath, int _size)
//    {
//        ObjectPool pool = gameObject.AddComponent<ObjectPool>();
//        pool.InitializePool(Resources.Load<GameObject>(_monsterPath), _size);
//    }

//    IEnumerator Spawn(string _type, int _count)
//    {
//        int curCount = 0;

//        while (curCount < _count)
//        {
//            bool isValidPos = false;
//            float xPos = 0, yPos = 0;

//            while (!isValidPos)
//            {
//                xPos = Random.Range(minXPos, maxXPos);
//                yPos = Random.Range(minYPos, maxYPos);
//                isValidPos = IsValidSpawnPosition(Character.transform.position, new Vector3(xPos, yPos, 0));
//            }

//            transform.position = Character.transform.position + new Vector3(xPos, yPos, 0);

//            GameObject obj = base.Spawn(_type, new Vector3(xPos, yPos));
//            curCount++;

//            yield return new WaitForSeconds(1f);
//        }
//    }

//    bool IsValidSpawnPosition(Vector3 _targetPos, Vector3 _position)
//    {
//        if (Vector3.Distance(_position, _targetPos) > 9)
//            return true;

//        return false;
//    }
//}
#endregion


public class SurvivalSpawner : MonoBehaviour
{
    [SerializeField]
    private List<Transform> m_SpawnPositions;

    [SerializeField]
    private PoolContainer m_PoolContainer;
    private GameMode m_GameMode;

    private StageData m_StageData;
    private List<Character> m_activateEnemies = new();

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Initialize(StageData stageData)
    {
        m_StageData = stageData;
        HashSet<int> uniqueEnemyIDs = stageData.GetSpawnableEnemyIDs();

        foreach (int ID in uniqueEnemyIDs)
        {
            ObjectPool Pool = gameObject.AddComponent<ObjectPool>();

            CharacterDataAsset CAD = Database.Instance.FindCharacterAsset(ID);
            Pool.InitializePool(AssetLoader.GetHandleInstance<GameObject>(CAD.PrefabKey), 100);

            m_PoolContainer.RegisterPool(Pool);
        }

        gameObject.SetActive(true);
    }

    public void AllKill()
    {
        foreach(Character enemy in m_activateEnemies)
        {
            if(enemy != null)
            {
                enemy.ForceKill();
            }
        }

        m_activateEnemies.Clear();
        StopAllCoroutines();
    }

    public IEnumerator SpawnEnemy(int waveIndex)
    {
        GameMode mode = GameManager.Instance.GameMode;
        GameRule rule = mode.Rule;

        int spawnLimit = (waveIndex + 1) * 5;

        float elapsedTime = 0f;
        int spawnCount = 0;

        List<CharacterDataAsset> spawnCDAs = m_StageData.WaveInfo[waveIndex].m_SpawnEnemies;

        while (true)
        {
            if (spawnCount >= spawnLimit)
                break;

            elapsedTime += Time.deltaTime;
            int randomLocationIndex = UnityEngine.Random.Range(0, m_SpawnPositions.Count);
            int spawnEnemyIndex = UnityEngine.Random.Range(0, m_StageData.WaveInfo[waveIndex].m_SpawnEnemies.Count);

            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * 3f;
            Vector3 circleOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);

            Vector3 position = m_SpawnPositions[randomLocationIndex].position + circleOffset;
            string targetName = spawnCDAs[spawnEnemyIndex].Name;

            ObjectPool pool = rule.PoolContainer.GetPool("Effect_SpawnVisualizer");
            GameObject visualizerGO = pool.GetObject(position, Quaternion.identity);

            if(visualizerGO)
            {
                SpawnVisualizer spawnVisualizer = visualizerGO.GetComponentInChildren<SpawnVisualizer>();
                spawnVisualizer.Initialize(position, () => {
                    var enemyPool = m_PoolContainer.GetPool($"{targetName}");
                    GameObject tempGO = enemyPool.GetObject(position, Quaternion.identity);
                    m_activateEnemies.Add(tempGO.GetComponent<Character>());

                    Character character = GameManager.Instance.GameMode.PlayerController.Character;
                    if (character)
                    {
                        AIBrain brain = tempGO.GetComponent<AIBrain>();
                        brain.SetTarget(character.gameObject);
                    }
                });
            }
        
            spawnCount++;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
