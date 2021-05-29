using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{

    //maybe should be agent factory?

    public GameObject agentPrefab;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void placeAgent(Tile tile)
    {
        Instantiate(agentPrefab, tile.transform.position, Quaternion.identity, this.transform);
    }
}
