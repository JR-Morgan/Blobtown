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
    public bool HasGoal => Goal != null;

    List<Tile> IPathFollower.Path { get; set; }

    void IPathFollower.GoalCompleteHandler(Tile completedGoal)
    {
        Goal = null;
    }

    #endregion

    [SerializeField]
    private float rotationSpeed = 1f;



    #region References to scene objects
    [SerializeField]
    private TownCenter _townCenter;
    public TownCenter TownCenter { get => _townCenter; set => _townCenter = value; }

    public Building Home { get; set; }

    public Tile Tile { get; private set; }
    public List<Tile> AdjacentTiles { get; private set; }
    #endregion

    #region Inventory
    [SerializeField]
    private Inventory _inventory;
    public Inventory Inventory { get => _inventory; private set => _inventory = value; }
    #endregion

    #region Agent Actor

    private AgentActor agentActor;


    [SerializeField]
    private AgentType _agentType;

    [DisplayProperty]
    public AgentType AgentType { get => _agentType;
        set
        {
            _agentType = value;
            agentActor = AgentActorFactory.CreateActor(this, _agentType);
        }
    }
    #endregion

    private void Awake()
    {
        Inventory = new Inventory();
        this.name = AgentNames.GetRandomName();
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
        agentActor = AgentActorFactory.CreateActor(this, AgentType);
    }

    private void Update()
    {
        Tile = Tile.Grid.TileAtWorldPosition(transform.position);
        AdjacentTiles = Tile.GetAdjacentTiles();

        foreach (Tile t in AdjacentTiles) t.Discovered = true;

        if (HasGoal)
        {
            Vector3 velocity = PathFollowHelper.CalculateDesiredVelocity(this);
            transform.position += velocity;
            if(velocity.sqrMagnitude > 0) transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(velocity), rotationSpeed * Time.deltaTime);
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




}
