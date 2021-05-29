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

    public Building home;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            agentActor.Act();
        }
    }




    public void SetDestination(Tile tile)
    {


        //navAgent.SetDestination(tile.transform.position);
    }

    public void MoveAgent(Tile tile)
    {
        Vector3 currentTilePos = transform.position;
        transform.position = Vector3.Lerp(currentTilePos, tile.transform.position, 1);
    }

    //public bool HasDestination => !Vector3.Equals(navAgent.destination, transform.position);

}
