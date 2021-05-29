using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase, DisallowMultipleComponent]
[AddComponentMenu("Simulation/Agent")]
//[RequireComponent(typeof(NavMeshAgent))]
public class AgentAI : MonoBehaviour
{

    private AgentActor agentActor;
    //private NavMeshAgent navAgent;

    public GameObject home;
    public int Carried{ get; set; }
    
    private void Awake()
    {
        //navAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        Initialise();
        
    }


    private void Initialise()
    {
        agentActor = AgentActorFactory.CreateActor(this);
    }

    private void Update()
    {
        agentActor.Act();
    }

    public void SetDestination(Vector3 destinationObject)
    {
        Tile tile = TileGrid.Instance.TileAtWorldPosition(destinationObject);

        //navAgent.SetDestination(tile.transform.position);
    }

    //public bool HasDestination => !Vector3.Equals(navAgent.destination, transform.position);

}
