using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("Simulation/Grid")]
public class TileGrid : Singleton<TileGrid>
{
    [SerializeField, HideInInspector]
    private Vector2 tileSize;
    public Tile[,] Tiles { get; set; }


    #region Helper Members
    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);
    public Vector2Int Dimensions => new Vector2Int(Width, Height);
    public Vector2 TileSize => tileSize;

    public bool IsInBounds(int x, int y) => x > 0 && y > 0 && x < Width && y < Height;

    #endregion

    #region Initialise
    public void InitialiseGrid(Tile[,] tiles, Vector2 tileSize)
    {
        this.Tiles = tiles;
        this.tileSize = tileSize;
    }

    #endregion

    #region Transforms

    private (int x, int y) TileIndexAtLocalPosition(Vector3 localPosition)
    {
        return (Mathf.FloorToInt(localPosition.x / tileSize.x), Mathf.FloorToInt(localPosition.y / tileSize.y));
    }

    /// <param name="worldPosition"></param>
    /// <returns>Tile at world position, <c>null</c> if none is found</returns>
    public Tile TileAtWorldPosition(Vector3 worldPosition)
    {
        Vector3 localPosition = this.transform.InverseTransformPoint(worldPosition);

        (int x, int y) = TileIndexAtLocalPosition(localPosition);

        if (!IsInBounds(x, y)) return null;

        return Tiles[x, y];
    }

    /// <param name="worldPosition"></param>
    /// <param name="tile"></param>
    /// <returns></returns>
    public bool TryGetTileAtWorldPosition(Vector3 worldPosition, out Tile tile)
    {
        tile = TileAtWorldPosition(worldPosition);
        return tile != null;
    }


    #endregion

    public Tile this[int x, int y] => Tiles[x,y];

    public List<Tile> GetAdjacentTiles(Tile t)
    {
        Vector3 localPosition = this.transform.InverseTransformPoint(t.transform.position);

        (int tileX, int tileY) = TileIndexAtLocalPosition(localPosition);

        var tiles = new List<Tile>();
        for (int x = tileX - 1; x <= tileX + 1; x++)
        { 
            for (int y = tileY - 1; y <= tileY + 1; y++)
            {
                if (IsInBounds(x, y))
                {
                    tiles.Add(Tiles[x,y]);
                }
            }
        }

        return tiles;
    }

    #region Casts
    public static explicit operator Tile[,](TileGrid g) => g.Tiles;
    #endregion
}

public class Tile : MonoBehaviour
{
    public TileGrid Grid { get; internal set; }
}
