using System.Collections.Generic;
using UnityEngine;

public class DefenceAIController : MonoBehaviour
{
    [SerializeField] MovementComponent movement;

    private void Start()
    {
        movement.MoveToDirection(new Vector2(-1,0));
    }

    void Update()
    {

    }


}




/* Old Defece
public List<Transform> DestinationList = new List<Transform>();

    Vector2 curDestination = Vector2.zero;
    int DestinationIndex = 1;

private void Start()
{
    curdestination = destinationlist[destinationindex].position;
    movement.movetolocation(curdestination, changedestination);
}

void ChangeDestination()
{
    DestinationIndex = (DestinationIndex + 1) % DestinationList.Count;
    curDestination = DestinationList[DestinationIndex].position;

    movement.MoveToLocation(curDestination, ChangeDestination);
}
*/