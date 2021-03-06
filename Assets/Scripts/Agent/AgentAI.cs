using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    [SerializeField]
    private Image progressBar;

    [SerializeField]
    private Canvas progressCanvas;

    public Image ProgressBar { get { return progressBar; } }

    public Canvas ProgressCanvas { get { return progressCanvas; } }

    public float Progress { get; set; }

    private Transform cameraTransform;

    [SerializeField]
    private Renderer meshRenderer;


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
            this.Goal = null;
            _agentType = value;
            meshRenderer.material.SetColor("_Color", FindColor(_agentType));
            agentActor = AgentActorFactory.CreateActor(this, _agentType);
        }
    }

    private Color FindColor(AgentType agentType) => agentType switch
    {
        AgentType.Miner => new Color(208, 96, 189) /255,
        AgentType.WoodCutter => new Color(238, 182, 87) / 255,
        AgentType.Scout => new Color(128, 246, 246) / 255,
        _ => new Color(24, 90, 15) / 255,
    };

    #endregion

    private void Awake()
    {
        Inventory = new Inventory();
        this.name = AgentNames.GetRandomName();
        //navAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        cameraTransform = Camera.main.transform;
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
        progressCanvas.transform.rotation = cameraTransform.rotation;

        Tile newTile = Tile.Grid.TileAtWorldPosition(transform.position);
        if (newTile != Tile)
        {
            Tile = newTile;
            Progress = 0f;
            progressBar.fillAmount = 0;
            progressCanvas.gameObject.SetActive(false);
        }
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
