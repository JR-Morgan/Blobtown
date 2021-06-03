using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputManager : Singleton<InputManager>
{
    private AgentFactory agentFactory;
    private BuildingFactory buildingFactory;
    private WorldSetUp worldSetUp;

    public UnityEvent<Selectable> OnSelectableChange;

    private void Start()
    {
        buildingFactory = BuildingFactory.Instance;
        agentFactory = AgentFactory.Instance;
        worldSetUp = WorldSetUp.Instance;
    }

    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && Time.timeScale > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 512f))
                {
                    if (hit.collider.TryGetComponent(out Selectable s))
                    {
                        if (!(s.TryGetComponent(out Tile tile) && !tile.Discovered))
                        {
                            Debug.Log($"Selecting {s.name}");
                            OnSelectableChange.Invoke(s);
                        }
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 512f))
                {
                    Tile tile = TileGrid.Instance.TileAtWorldPosition(hit.collider.transform.position);
                    if (tile != null)
                    {
                        //agentFactory.PlaceAgent(tile);
                        buildingFactory.CreateBuilding(BuildingType.Farm, tile, worldSetUp.initialTownCenter);
                    }
                }
            }

        }


        //if (Input.GetMouseButtonDown(1))
        //{
        //    Tile tile = TileGrid.Instance.TileAtWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //    if (tile != null)
        //    {
        //        BuildingType b = BuildingType.Home;
        //        if (buildingFactory.TownCenter == null)
        //        {
        //            buildingFactory.CreateBuilding(b, tile);
        //        }
        //        else
        //        {
        //            buildingFactory.CreateBuilding(b);
        //        }
        //    }
        //}
    }
}
