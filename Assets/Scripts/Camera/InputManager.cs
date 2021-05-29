using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public GameObject agentPrefab;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Tile tile = TileGrid.Instance.TileAtWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (tile != null)
            {
                Instantiate(agentPrefab, tile.transform.position, Quaternion.identity);
            }

        }
    }
}
