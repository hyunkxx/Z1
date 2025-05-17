using System.Collections.Generic;
using UnityEngine;

public class DefenceAIController : MonoBehaviour
{
    [SerializeField] MovementComponent movement;
    public List<Transform> DestinationList = new List<Transform>();

    Vector2 curDestination = Vector2.zero;
    int DestinationIndex = 0;

    

    private void Start()
    {
        curDestination = DestinationList[DestinationIndex].position;
    }

    void Update()
    {
        movement.MoveToLocation(curDestination);
        ChangeDestination();
    }

    void ChangeDestination()
    {
        if (Vector2.Distance(transform.position, curDestination) < 0.5f)
        {
            DestinationIndex++;

            if (DestinationIndex == DestinationList.Count)
                DestinationIndex = 0;

            if(DestinationList.Count > 0)
            curDestination = DestinationList[DestinationIndex].position;
        }
    }
}
