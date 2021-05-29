using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeManager : Singleton<HomeManager>
{

    //maybe should be home factory?

    public Building homePrefab;
    private List<Building> homes;
    private Building townCentre;


    // Start is called before the first frame update
    void Start()
    {
        homes = new List<Building>();
        foreach (Transform child in transform) 
        {
            homes.Add(child.gameObject.GetComponent<Building>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Building BuildHome(Tile tile)
    {
        GameObject newHome = Instantiate(homePrefab.gameObject, tile.transform.position, Quaternion.identity, this.transform);
        if (townCentre == null)
        {
            townCentre = newHome.GetComponent<Building>();
        }
        homes.Add(newHome.GetComponent<Building>());
        return newHome.GetComponent<Building>();
    }

    public Tile NextHomeTile()
    {
        if (townCentre == null)
        {
            return null;
        }
        else 
        {
            List<Tile> shuffle = townCentre.GetAdjacentTiles();
            shuffle.Shuffle();

            foreach (Tile tile in shuffle)
            {
                if (IsBuildingSpaceFree(tile))
                {
                    return tile;
                }
            }
            return null;
        }
    }

    private bool IsBuildingSpaceFree(Tile originTile)
    {
        
        List<Tile> buildingSpace = BuildingSpace(originTile);
        foreach (Tile tile in buildingSpace)
        {
            if (tile.HasBuilding)
            {
                return false;
            }
        }
        return true;
    }

    private List<Tile> BuildingSpace(Tile tile)
    {
        List<Tile> tilesUsed = new List<Tile>();
        for (int x = 0; x < homePrefab.size.x; x++)
        {
            for (int y = 0; y < homePrefab.size.y; y++)
            {
                tilesUsed.Add(TileGrid.Instance.Tiles[x, y]);
            }
        }
        return tilesUsed;
    }
}
