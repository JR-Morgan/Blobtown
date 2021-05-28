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
        Tile[,] tiles = GenerateLevel(gridSize.x, gridSize.y, tileSize, this.transform, tilePrefab);
        TileGrid grid = GetComponent<TileGrid>();

        grid.InitialiseGrid(tiles, tileSize);
    }


    public static Tile[,] GenerateLevel(int width, int height, Vector2 tileSize, Transform parent = null, GameObject tilePrefab = null)
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
                go.transform.parent = parent;
                go.transform.position = new Vector3(x * tileSize.x, y * tileSize.y);


                tiles[x, y] = t;
            }
        }

        return tiles;
    }
}
