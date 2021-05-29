using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public GameObject agentPrefab;
    public HomeManager homeManager;
    public AgentManager agentManager;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Tile tile = TileGrid.Instance.TileAtWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (tile != null)
            {
                agentManager.placeAgent(tile);
            }

        }

        if (Input.GetMouseButtonDown(1))
        {
            Tile tile = TileGrid.Instance.TileAtWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (tile != null)
            {
                homeManager.placeHome(tile);
            }
        }
    }
}
