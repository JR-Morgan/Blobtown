using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileGenerator
{
    public static Tile[,] GenerateLevel(int width, int height)
    {
        Tile[,] tiles = new Tile[width, height];

        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < width; y++)
            {
                GameObject go = new GameObject();
                Tile tile = go.AddComponent<Tile>();

                //Setup Tile here



                tiles[x, y] = tile;
            }
        }

        return tiles;
    }
}
