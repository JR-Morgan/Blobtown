using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAI : MonoBehaviour
{

    private AgentActor agentActor;
    public GameObject home;

    private void Start()
    {
        Initialise();
        
    }


    private void Initialise()
    {
        agentActor = AgentActorFactory.CreateActor();
    }

    private void Update()
    {
        agentActor.Act();
    }

    public void SetDestination(Vector3 destinationObject)
    {
        TileGrid.Instance.TileAtWorldPosition(destinationObject);
    }

}
