using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase, DisallowMultipleComponent]
[AddComponentMenu("Simulation/Agent")]
//[RequireComponent(typeof(NavMeshAgent))]
public class AgentAI : MonoBehaviour, IPathFollower
{

    #region IPathFollower
    [SerializeField]
    private Tile _goal;
    public Tile Goal { get => _goal;
        set
        {
            _goal = value;

        }
    }

    [SerializeField]
    private float _speed;
    public float Speed { get => _speed; set =>_speed = value; }

    List<Tile> IPathFollower.Path { get; set; }

    void IPathFollower.GoalCompleteHandler(Tile completedGoal)
    {
        Goal = null;
    }

    #endregion

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
        if(HasGoal)
        {
            transform.position += PathFollowHelper.CalculateDesiredVelocity(this);
        }
        else
        {
            agentActor.Act();
        }
    }




    public void MoveAgent(Tile tile)
    {
        Vector3 currentTilePos = transform.position;
        transform.position = Vector3.Lerp(currentTilePos, tile.transform.position, 1);
    }



    public bool HasGoal => Goal != null;

}
