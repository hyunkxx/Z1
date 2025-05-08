using System.Collections;
using UnityEngine;

public class SurvivalSpawner : SpawnController
{
    public PlayerController playerController;
    public GameObject Character;

    private void Awake()
    {
    }
    void Start()
    {
        Invoke("Initialize", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Initialize()
    {
        Character = playerController.Character.gameObject;

        foreach (var pool in objectPools.GetContainer())
        {
            StartCoroutine(Spawn(pool.Key, pool.Value.PoolSize));
        }
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
                xPos = Random.Range(-10, 10);
                yPos = Random.Range(-6, 6);
                isValidPos = IsValidSpawnPosition(Character.transform.position, new Vector3(xPos, yPos, 0));
            }

            transform.position = Character.transform.position + new Vector3(xPos, yPos, 0);

            GameObject obj = base.Spawn(_type, new Vector3(xPos, yPos));
            obj.GetComponent<MonsterStateMachine>().target = Character;
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
