using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase, DisallowMultipleComponent]
public class Building : MonoBehaviour
{
    public BuildingType BuildingType { get; set; }

    [SerializeField]
    private Tile _position;
    public Tile Position { get => _position; set => _position = value; }

    [SerializeField]
    private Vector2Int _size; 
    public Vector2Int Size { get => _size; set => _size = value; }

    #region Inventory
    [SerializeField]
    private Inventory _inventory;
    public Inventory Inventory { get => _inventory; private set => _inventory = value; }
    #endregion

    [SerializeField]
    private bool _isTownCenter;
    public bool IsTownCenter { get => _isTownCenter; set => _isTownCenter = value; }


    private void Awake()
    {
        Inventory = new Inventory();
    }

    // Start is called before the first frame update
    void Start()
    {
        this.Position = TileGrid.Instance.TileAtWorldPosition(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Tile> GetAdjacentTiles()
    {
        
        List<Tile> adjTiles = new List<Tile>();

        Vector2Int bottomLeft = Position.GridIndex + new Vector2Int(-1, -1);
        Vector2Int topLeft = Position.GridIndex + new Vector2Int(0, Size.y) + new Vector2Int(-1, 0);
        Vector2Int topRight = Position.GridIndex + new Vector2Int(Size.x, Size.y) + new Vector2Int(0, 0);
        Vector2Int bottomRight = Position.GridIndex + new Vector2Int(Size.x, 0) + new Vector2Int(0, -1);

        for (int y = bottomLeft.y; y <= topLeft.y; y++)
        {
            if (TileGrid.Instance.IsInBounds(bottomLeft.x, y))
            {
                if (!adjTiles.Contains(TileGrid.Instance.Tiles[bottomLeft.x, y]))
                {
                    adjTiles.Add(TileGrid.Instance.Tiles[bottomLeft.x, y]);
                }
            }
            
        }

        for (int y = bottomRight.y; y <= topRight.y; y++)
        {
            if (TileGrid.Instance.IsInBounds(bottomRight.x, y))
            { 

                if (!adjTiles.Contains(TileGrid.Instance.Tiles[bottomRight.x, y]))
                {
                    adjTiles.Add(TileGrid.Instance.Tiles[bottomRight.x, y]);
                }
            
            }

        }

        for (int x = bottomLeft.x; x <= bottomRight.x; x++)
        {
            if (TileGrid.Instance.IsInBounds(x, bottomRight.y))
            {
                if (!adjTiles.Contains(TileGrid.Instance.Tiles[x, bottomRight.y]))
                {
                    adjTiles.Add(TileGrid.Instance.Tiles[x, bottomRight.y]);
                }

                
            }

        }

        for (int x = topLeft.x; x <= topRight.x; x++)
        {
            if (TileGrid.Instance.IsInBounds(x, topRight.y))
            {
                if (!adjTiles.Contains(TileGrid.Instance.Tiles[x, topRight.y]))
                {
                    adjTiles.Add(TileGrid.Instance.Tiles[x, topRight.y]);
                }
                
            }

        }

        return adjTiles;
    }
}
