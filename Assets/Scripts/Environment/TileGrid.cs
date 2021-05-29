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
    [SerializeField, HideInInspector]
    private int _width, _height;
    public int Width { get => _width; private set => _width = value; } 
    public int Height { get => _height; private set => _height = value; }
    public Vector2Int Dimensions => new Vector2Int(Width, Height);
    public Vector2 TileSize => tileSize;

    public bool IsInBounds(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

    #endregion


    protected override void Awake()
    {
        base.Awake();

        //Finds tiles in children and reconstructs Tiles array since it is not serialied
        if(Tiles == null)
        {
            Tiles = new Tile[Width, Height];
            Tile[] children = GetComponentsInChildren<Tile>();

            Debug.Assert(children.Length == Width * Height, $"{typeof(TileGrid)} failed to reconstruct {typeof(Tile)} array after scene reload", this);

            foreach (Tile t in children)
            {
                Vector2Int foo = t.GridIndex;
                Tiles[foo.x, foo.y] = t;
            }
        }
    }

    #region Initialise
    public void InitialiseGrid(Tile[,] tiles, Vector2 tileSize)
    {
        this.Tiles = tiles;
        this.tileSize = tileSize;
        Width = Tiles.GetLength(0);
        Height = Tiles.GetLength(1);
    }

    #endregion

    #region Transforms

    private (int x, int y) TileIndexAtLocalPosition(Vector3 localPosition)
    {
        return (Mathf.RoundToInt(localPosition.x / tileSize.x), Mathf.RoundToInt(localPosition.z / tileSize.y));
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
        int tileX = t.GridIndex.x;
        int tileY = t.GridIndex.y;

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

