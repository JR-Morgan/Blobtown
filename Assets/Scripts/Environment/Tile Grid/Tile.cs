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

    #region Breadcrumbs
    [SerializeField]
    private float _breadCrumbs;
    public float BreadCrumbs {
        get => _breadCrumbs;
        set => _breadCrumbs = Mathf.Max(value, 0);
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


    public TileType TileType { get => _tileType;
        set
        {
            _tileType = value;
            Initialise(_tileType);
        }
    }
    public Building Building { get; set; }
    public bool HasBuilding => Building != null;

    Renderer _renderer;

    private void Awake()
    {
         this.RequireComponentInChildren(out _renderer);
    }

    private void Start()
    {
        Initialise(_tileType);
    }

    private void OnValidate()
    {
        Initialise(_tileType);
    }

    private void Initialise(TileType tileType)
    {
        if (resource != null) Destroy(resource);

        if (TileManager.IsSingletonInitialised)
        {
            _tileData = TileManager.Instance.GetResourceData(tileType);

            if (TileData != null && TileData.resourcePrefab != null)
            {
                resource = Instantiate(TileData.resourcePrefab, transform);
            }
        }
    }

}

public enum TileType
{
    Default,
    Forest,
    Ore

}
