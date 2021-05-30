using System.Collections.Generic;
using System.Linq;
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

    public bool IsInBounds(Vector2Int xy) => IsInBounds(xy.x, xy.y);
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
            int expectedSize = Width * Height;
            Debug.Assert(children.Length == expectedSize, $"{typeof(TileGrid)} failed to reconstruct {typeof(Tile)} array after scene reload.\nExpected {expectedSize} {typeof(Tile)} children but there was {children.Length}.", this);

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
                if (x == t.GridIndex.x && y == t.GridIndex.y) continue;
                if (IsInBounds(x, y))
                {
                    tiles.Add(Tiles[x,y]);
                }
            }
        }

        return tiles;
    }

    #region In rectangle/circle

    public IEnumerable<Tile> TilesInRect(Tile tile, Vector2Int size) => TilesInIndeces(IndeciesInRect(tile, size));
    public IEnumerable<Tile> TilesInRect(Vector2Int position, Vector2Int size) => TilesInIndeces(IndeciesInRect(position, size));

    public List<Vector2Int> IndeciesInRect(Tile tile, Vector2Int size) => IndeciesInRect(tile.GridIndex, size);
    public List<Vector2Int> IndeciesInRect(Vector2Int position, Vector2Int size)
    {
        List<Vector2Int> indecies = new List<Vector2Int>();
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                indecies.Add(new Vector2Int(x + position.x, y + position.y));
            }
        }
        return indecies;
    }

    public IEnumerable<Tile> TilesInIndeces(IEnumerable<Vector2Int> indecies)
    {
        return indecies.Where(i => IsInBounds(i)).Select(i => this[i.x, i.y]);
    }

    public IEnumerable<Tile> TilesInCircle(Tile t, float radius) => TilesInIndeces(IndeciesInCircle(t, radius));
    public IEnumerable<Tile> TilesInCircle(Vector2Int center, int radius) => TilesInIndeces(IndeciesInCircle(center, radius));

    public List<Vector2Int> IndeciesInCircle(Tile t, float radius) => IndeciesInCircle(t.GridIndex, radius);
    public List<Vector2Int> IndeciesInCircle(Vector2Int center, float radius)
    {
        List<Vector2Int> tmpList = new List<Vector2Int>();
        List<Vector2Int> list = new List<Vector2Int>();
        double rSquared = radius * radius; // using sqared reduces execution time (no square root needed)
        for (int x = 1; x <= radius; x++)
            for (int y = 0; y <= radius; y++)
            {
                Vector2Int v = new Vector2Int(x, y);
                if (v.sqrMagnitude <= rSquared)
                    tmpList.Add(v);
                else
                    break;
            }

        list.Add(center);

        foreach (Vector2Int v in tmpList)
        {
            Vector2Int vMirr = new Vector2Int(v.x, -1 * v.y);
            list.Add(center + v);
            list.Add(center - v);
            list.Add(center + vMirr);
            list.Add(center - vMirr);
        }


        return list;
    }
    #endregion

    #region Casts
    public static explicit operator Tile[,](TileGrid g) => g.Tiles;
    #endregion
}

