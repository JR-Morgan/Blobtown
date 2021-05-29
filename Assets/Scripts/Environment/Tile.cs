using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Tile : MonoBehaviour
{
    #region Grid References
    [SerializeField, HideInInspector]
    private TileGrid _grid;
    public TileGrid Grid { get => _grid; internal set => _grid = value; }

    [SerializeField]
    private Vector2Int _gridIndex;
    public Vector2Int GridIndex { get => _gridIndex; internal set => _gridIndex = value; }
    #endregion

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
    Renderer _renderer;

    private void Awake()
    {
         this.RequireComponentInChildren(out _renderer);
    }

    private void Initialise(TileType type)
    {
        //Temporary for now

        Color c = type switch
        {
            TileType.Ore => Color.grey,
            _ => Color.white,
        };

        _renderer.material.SetColor("_Color", c);
    }

}

public enum TileType
{
    Default,
    Forest,
    Ore

}
