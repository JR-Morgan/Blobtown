using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

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

        Cluster(tiles);

        return tiles;
    }


    private void Cluster(Tile[,] tiles)
    {
        List<(int, int)> resources = new List<(int, int)>();
        for (int i = 0; i < tiles.GetLength(0); i++)
        {

            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if (tiles[i, j].TileType != TileType.Default)
                {
                    resources.Add((i, j));
                }
            }

        }

        foreach ((int, int) t in resources)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int a = t.Item1 + i;
                    int b = t.Item2 + j;
                    if (a >= 0 && b >= 0 && a <= tiles.GetLength(0)-1 && b <= tiles.GetLength(1)-1)
                    {
                        tiles[a, b].TileType = tiles[t.Item1, t.Item2].TileType;
                    }
                }
            }


        }
    }
        


}
