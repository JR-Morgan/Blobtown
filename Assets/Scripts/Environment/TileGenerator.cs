using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(TileGrid))]
[AddComponentMenu("Simulation/Grid Generator")]
public class TileGenerator : MonoBehaviour
{
    [SerializeField]
    private int tileLayer;

    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private Vector2Int gridSize;
    [SerializeField]
    private Vector2 tileSize;

    public void Awake()
    {
        Generate();
    }


    public void Generate()
    {
        TileGrid grid = GetComponent<TileGrid>();
        Tile[,] tiles = GenerateLevel(grid, gridSize.x, gridSize.y, tileSize, tileLayer, this.transform, tilePrefab);

        grid.InitialiseGrid(tiles, tileSize);
    }


    public static Tile[,] GenerateLevel(TileGrid grid, int width, int height, Vector2 tileSize, int layer, Transform parent = null, GameObject tilePrefab = null)
    {
        Tile[,] tiles = new Tile[width, height];

        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                GameObject go = tilePrefab == null ? new GameObject() : Instantiate(tilePrefab);

                if(!go.TryGetComponent(out Tile t)) t = go.AddComponent<Tile>();

                //Setup Tile here
                go.name = $"Tile {x},{y}";
                go.layer = layer;
                go.transform.parent = parent;
                go.transform.position = new Vector3(x * tileSize.x, 0f, y * tileSize.y);

                t.Grid = grid;

                tiles[x, y] = t;
            }
        }

        return tiles;
    }
}
