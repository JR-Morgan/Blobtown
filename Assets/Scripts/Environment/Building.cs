using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    public Vector2Int size;


    // Start is called before the first frame update
    void Start()
    {
        GetAdjacentTiles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Tile> GetAdjacentTiles()
    {
        
        List<Tile> adjTiles = new List<Tile>();

        Vector2Int bottomLeft = TileGrid.Instance.TileAtWorldPosition(transform.position).GridIndex + new Vector2Int(-1, -1);
        Vector2Int topLeft = TileGrid.Instance.TileAtWorldPosition(transform.position).GridIndex + new Vector2Int(0, size.y) + new Vector2Int(-1, 0);
        Vector2Int topRight = TileGrid.Instance.TileAtWorldPosition(transform.position).GridIndex + new Vector2Int(size.x, size.y) + new Vector2Int(0, 0);
        Vector2Int bottomRight = TileGrid.Instance.TileAtWorldPosition(transform.position).GridIndex + new Vector2Int(size.x, 0) + new Vector2Int(0, -1);

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

        Debug.Log(adjTiles.Count);

        return adjTiles;
    }
}
