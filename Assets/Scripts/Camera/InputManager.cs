using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private GameObject agentPrefab;

    public AgentManager agentManager;
    private BuildingFactory buildingFactory;

    private void Start()
    {
        buildingFactory = BuildingFactory.Instance;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Tile tile = TileGrid.Instance.TileAtWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (tile != null)
            {
                agentManager.PlaceAgent(tile);
            }

        }

        if (Input.GetMouseButtonDown(1))
        {
            Tile tile = TileGrid.Instance.TileAtWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (tile != null)
            {
                BuildingType b = BuildingType.Home;
                if (buildingFactory.TownCenter == null)
                {
                    buildingFactory.CreateBuilding(b, tile);
                }
                else
                {
                    buildingFactory.CreateBuilding(b);
                }
            }
        }
    }
}
