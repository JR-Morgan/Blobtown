using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public partial class Tile : MonoBehaviour
{
    #region Grid References
    [SerializeField, HideInInspector]
    private TileGrid _grid;
    public TileGrid Grid { get => _grid; internal set => _grid = value; }

    [SerializeField]
    private Vector2Int _gridIndex;
    public Vector2Int GridIndex { get => _gridIndex; internal set => _gridIndex = value; }

    public List<Tile> GetAdjacentTiles() => Grid.GetAdjacentTiles(this);
    #endregion

    #region Discovered
    [SerializeField]
    private bool _discovered;

    [DisplayProperty]
    public bool Discovered { get => _discovered;
        set {
            if(_discovered != value)
            {
                _discovered = value;
                DiscoveredChangeHandler();
            }
        }
    }

    private void DiscoveredChangeHandler()
    {
        foreach(Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = _discovered;
        }
    }
    #endregion

    [SerializeField]
    private TileType _tileType;
    private TileData _tileData;
    public TileData TileData
    {
        get
        {
            return _tileData != null ? _tileData : _tileData = TileManager.Instance.GetResourceData(_tileType);
        }
    }

    [SerializeField, HideInInspector]
    private GameObject resource;

    [DisplayProperty]
    public TileType TileType { get => _tileType;
        set
        {
            _tileType = value;
#if UNITY_EDITOR
            if (Application.isPlaying) Initialise(_tileType);
#else
            Initialise(_tileType);
#endif
        }
    }
    public Building Building { get; set; }
    public bool HasBuilding => Building != null;


    private void Start()
    {
        Initialise(_tileType);
    }

    private void OnValidate()
    {
        //DiscoveredChangeHandler();
        //Initialise(_tileType);
    }

    private void Initialise(TileType tileType)
    {
        
        if (resource != null)
        {
            Destroy(resource);
        }

        if (TileManager.IsSingletonInitialised)
        {
            _tileData = TileManager.Instance.GetResourceData(tileType);

            if (TileData != null && TileData.resourcePrefab != null)
            {
                var r = new System.Random(this.GetHashCode());
                resource = Instantiate(TileData.resourcePrefab, transform.position, Quaternion.Euler(0, (float)r.NextDouble() * 360f,0), transform);;
            }
        }

        DiscoveredChangeHandler();
    }

}

public enum TileType
{
    Default,
    Forest,
    Ore

}
