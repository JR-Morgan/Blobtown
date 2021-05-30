using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{

    public GameObject agentPrefab;
    public HomeManager homeManager;
    public AgentManager agentManager;
    public TabGroup tabGroup;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                tabGroup.ResetSelectedTab();


                Tile tile = TileGrid.Instance.TileAtWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if (tile != null)
                {
                    agentManager.placeAgent(tile);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Tile tile = TileGrid.Instance.TileAtWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (tile != null)
            {
                homeManager.BuildHome(tile);
            }
        }
    }
}
