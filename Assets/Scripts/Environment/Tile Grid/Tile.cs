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

    [SerializeField]
    private TileType _tileType;
    public TileType TileType { get => _tileType;
        set
        {
            if(_tileType != value)
            {
                _tileType = value;
               Initialise(_tileType);
            }
        }
    }

    public bool HasBuilding { get; internal set; }

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
    private void Initialise(TileType type)
    {
        Color c = type switch
        {
            TileType.Ore => Color.grey,
            TileType.Forest => Color.green,
            _ => Color.white,
        };

        if(_renderer != null)
        {
            _renderer.material.SetColor("_Color", c);
        }
    }

}

public enum TileType
{
    Default,
    Forest,
    Ore

}
