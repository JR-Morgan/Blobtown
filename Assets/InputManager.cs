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
            Ray ray = new Ray(Camera.main.transform.position,Vector3.down);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit)) 
            { 
                Vector3 hitPosition = hit.point;
                Debug.Log(hitPosition);
                Tile tile = TileGrid.Instance.TileAtWorldPosition(hitPosition);
                Debug.Log(tile);
                Instantiate(agentPrefab, tile.transform.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("no hit");
            }
            
        }
    }
}
