using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnerPool : MonoBehaviour
{
    [SerializeField]
    private List<Transform> m_SpawnPositions;

    [SerializeField]
    private PoolContainer m_PoolContainer;
    private GameMode m_GameMode;

    private StageData m_StageData;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Initialize(StageData stageData)
    {
        m_StageData = stageData;
        HashSet<int> uniqueEnemyIDs = stageData.GetSpawnableEnemyIDs();

        foreach(int ID in uniqueEnemyIDs)
        {
            ObjectPool Pool = gameObject.AddComponent<ObjectPool>();

            CharacterDataAsset CAD = Database.Instance.FindCharacterAsset(ID);
            Pool.InitializePool(AssetLoader.GetHandleInstance<GameObject>(CAD.PrefabKey), 100);

            m_PoolContainer.RegisterPool(Pool);
        }

        gameObject.SetActive(true);
        StartCoroutine(WaveLifeCycle());
    }

    IEnumerator WaveLifeCycle()
    {
        const float waveCycle = 60f;
        const float spawnCycle = 5f;

        float waveTimer = 0f;
        int currentWave = 0;

        while (currentWave < m_StageData.TotalWaveCount())
        {
            float spawnTimer = 0f;
            while (waveTimer < waveCycle)
            {
                waveTimer += Time.deltaTime;
                spawnTimer += Time.deltaTime;

                if (spawnTimer >= spawnCycle)
                {
                    spawnTimer = 0f;
                    StartCoroutine(SpawnEnemy(currentWave));
                }

                yield return null;
            }

            waveTimer = 0f;
            currentWave++;

            Debug.Log(currentWave);
        }
    }

    IEnumerator SpawnEnemy(int waveIndex)
    {
        int spawnLimit = (waveIndex + 1) * 5;

        float elapsedTime = 0f;
        int spawnCount = 0;

        List<CharacterDataAsset> spawnCDAs = m_StageData.WaveInfo[waveIndex].m_SpawnEnemies;

        while (true)
        {
            if(spawnCount >= spawnLimit)
                break;

            elapsedTime += Time.deltaTime;
            int randomLocationIndex = UnityEngine.Random.Range(0, m_SpawnPositions.Count);
            int spawnEnemyIndex = UnityEngine.Random.Range(0, m_StageData.WaveInfo[waveIndex].m_SpawnEnemies.Count);

            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle;
            Vector3 circleOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);

            Vector3 position = m_SpawnPositions[randomLocationIndex].position + circleOffset;
            string targetName = spawnCDAs[spawnEnemyIndex].Name;

            var pool = m_PoolContainer.GetPool($"{targetName}");
            GameObject tempGO = pool.GetObject(position, Quaternion.identity);

            Character character = GameManager.Instance.GameMode.PlayerController.Character;
            if(character)
            {
                AIBrain brain = tempGO.GetComponent<AIBrain>();
                brain.SetTarget(character.gameObject);
            }

            spawnCount++;

            yield return new WaitForSeconds(0.5f);
        }
    }
}
