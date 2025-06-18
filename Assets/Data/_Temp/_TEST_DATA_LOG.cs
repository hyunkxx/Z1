using UnityEngine;
using System.Collections.Generic;

public class _TEST_DATA_LOG : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var test = Database.Instance.Service.MakeDictionaryFromTable<CharacterStatsRecord>();
        }
    }
}
