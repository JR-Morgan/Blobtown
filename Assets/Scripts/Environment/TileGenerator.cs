using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(TileGrid))]
[AddComponentMenu("Simulation/Grid Generator")]
public class TileGenerator : MonoBehaviour
{

    [SerializeField]
    private int tileLayer;

    [Range(0,1)]
    [SerializeField]
    private float oreProabality;

    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private Vector2Int gridSize;
    [SerializeField]
    private Vector2 tileSize;

    public void Generate()
    {
        transform.DestroyChildren();

        TileGrid grid = GetComponent<TileGrid>();

        Tile[,] tiles = GenerateTiles(grid);

        grid.InitialiseGrid(tiles, tileSize);
    }


    public Tile[,] GenerateTiles(TileGrid grid)
    {
        int width = gridSize.x, height = gridSize.y;
        Tile[,] tiles = new Tile[width, height];

        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject go = tilePrefab == null
                    ? new GameObject()
#if UNITY_EDITOR
                    : (GameObject)PrefabUtility.InstantiatePrefab(tilePrefab);
#else
                    : Instantiate(tilePrefab);
#endif

                if (!go.TryGetComponent(out Tile t)) t = go.AddComponent<Tile>();

                //Setup Tile here
                go.name = $"Tile {x},{y}";
                go.layer = tileLayer;
                go.transform.parent = this.transform;
                go.transform.position = new Vector3(x * tileSize.x, 0f, y * tileSize.y);

                if (Random.value < oreProabality)
                {
                    t.TileType = TileType.Ore;
                }

                t.Grid = grid;
                t.GridIndex = new Vector2Int(x,y);

                tiles[x, y] = t;
            }
        }

        return tiles;
    }
}
