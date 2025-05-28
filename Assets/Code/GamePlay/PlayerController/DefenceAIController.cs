using System.Collections.Generic;
using UnityEngine;

public class DefenceAIController : MonoBehaviour
{
    [SerializeField] MovementComponent movement;
    public List<Transform> DestinationList = new List<Transform>();

    Vector2 curDestination = Vector2.zero;
    int DestinationIndex = 1;

    

    private void Start()
    {
        curDestination = DestinationList[DestinationIndex].position;
        movement.MoveToLocation(curDestination, ChangeDestination);
    }

    void Update()
    {
    }

    void ChangeDestination()
    {
        DestinationIndex++;

        if (DestinationIndex == DestinationList.Count)
            DestinationIndex = 0;

        if (DestinationList.Count > 0)
            curDestination = DestinationList[DestinationIndex].position;

        movement.MoveToLocation(curDestination, ChangeDestination);
    }
}
