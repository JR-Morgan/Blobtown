using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase, DisallowMultipleComponent]
[AddComponentMenu("Simulation/Agent")]
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
        //inventory.Contents.Clear(); //TODO for now, just clear their inventory
    }

    #endregion

    private AgentActor agentActor;
    //private NavMeshAgent navAgent;

    public Building Home { get; set; }

    #region Inventory
    [SerializeField]
    private Inventory _inventory;
    public Inventory Inventory { get => _inventory; private set => _inventory = value; }
    #endregion
    public Tile Tile { get; private set; }

    private void Awake()
    {
        Inventory = new Inventory();
        //navAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        Initialise();
        Tile = TileGrid.Instance.TileAtWorldPosition(this.transform.position);
        Debug.Assert(Tile != null, $"{typeof(AgentAI)} is starting on an invalid tile!");
    }


    private void Initialise()
    {
        agentActor = AgentActorFactory.CreateActor(this);
    }

    private void Update()
    {
        Tile = Tile.Grid.TileAtWorldPosition(transform.position);

        if (HasGoal)
        {
            transform.position += PathFollowHelper.CalculateDesiredVelocity(this);
        }
        else
        {
            agentActor.Act(Tile);
        }
    }




    public void MoveAgent(Tile tile)
    {
        Vector3 currentTilePos = transform.position;
        transform.position = Vector3.Lerp(currentTilePos, tile.transform.position, 1);
    }



    public bool HasGoal => Goal != null;

}
