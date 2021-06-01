using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[AddComponentMenu("Simulation/Factories/Agent Factory")]
public class AgentFactory : Singleton<AgentFactory>
{
    [SerializeField]
    private GameObject agentPrefab;

    public void PlaceAgent(Tile tile)
    {
        Instantiate(agentPrefab, tile.transform.position, Quaternion.identity, this.transform);
    }
}
