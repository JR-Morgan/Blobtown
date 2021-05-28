using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("Simulation/Grid")]
public class TileGrid : MonoBehaviour
{
    [SerializeField]
    private Vector2 tileSize;

    [SerializeField]
    private Tile[,] _tiles;
    public Tile[,] Tiles { get => _tiles; set => _tiles = value; }


    #region Helper Members
    public int Width => Tiles.GetLength(0);
    public int Height => Tiles.GetLength(1);
    public Vector2Int Dimensions => new Vector2Int(Width, Height);
    public Vector2 TileSize => tileSize;

    public bool IsInBounds(int x, int y) => x > 0 && y > 0 && x < Width && y < Height;

    #endregion

    #region Initialise
    private void InitialiseGrid(Tile[,] Tiles, Vector2 tileSize)
    {
        _tiles = Tiles;
        this.tileSize = tileSize;
    }

    public void InitialiseGrid(int gridWidth, int gridHeight, int tileWidth, int tileHeight) => InitialiseGrid(gridWidth, gridHeight, new Vector2(tileWidth, tileHeight));
    public void InitialiseGrid(int gridWidth, int gridHeight, Vector2 tileSize) => InitialiseGrid(TileGenerator.GenerateLevel(gridWidth, gridHeight), tileSize);
    #endregion

    #region Transforms

    /// <param name="worldPosition"></param>
    /// <returns>Tile at world position, <c>null</c> if none is found</returns>
    public Tile TileAtWorldPosition(Vector3 worldPosition)
    {
        Vector3 localPosition = this.transform.InverseTransformPoint(worldPosition);

        int x = Mathf.FloorToInt(localPosition.x / tileSize.x);
        int y = Mathf.FloorToInt(localPosition.y / tileSize.y);

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

    #region Casts
    public static explicit operator Tile[,](TileGrid g) => g.Tiles;
    #endregion
}

public class Tile : MonoBehaviour
{

}
