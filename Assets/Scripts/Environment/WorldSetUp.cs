using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSetUp : Singleton<WorldSetUp>
{

    [SerializeField]
    private int numOfAgents;

    public GameObject cameraTarget;

    public TownCenter initialTownCenter;

    void Start()
    {
        Tile tile = TileGrid.Instance.Tiles[(int)TileGrid.Instance.Width / 2, (int)TileGrid.Instance.Height / 2];
        
        
        

        initialTownCenter = BuildingFactory.Instance.CreateTownCenter(tile);

        for (int i = 0; i < numOfAgents; i++)
        {
            AgentFactory.Instance.PlaceAgent(initialTownCenter.Building.GetAdjacentTiles()[i]);
        }

        foreach (Tile buildingTile in TileGrid.Instance.TilesInRect(tile, initialTownCenter.Building.Size))
        {

            IEnumerable<Tile> tilesAroundTownCentre = TileGrid.Instance.TilesInCircle(buildingTile, 8);

            foreach (Tile tileAroundTownCentre in tilesAroundTownCentre)
            {
                tileAroundTownCentre.TileType = default;
            }
        }

        cameraTarget.transform.position = new Vector3(tile.transform.position.x, 0,tile.transform.position.y);
    }
}
