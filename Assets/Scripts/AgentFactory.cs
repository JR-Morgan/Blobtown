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
        GameObject go = Instantiate(agentPrefab, tile.transform.position, Quaternion.identity, this.transform);

        AgentAI agent = go.GetComponent<AgentAI>();

        Building townCenter = BuildingFactory.Instance.NearestBuildingOfType(tile.transform.position, BuildingType.TownCenter);

        if (townCenter == null)
        {
            townCenter = BuildingFactory.Instance.CreateBuilding(BuildingType.TownCenter, tile, null);
        }
        agent.TownCenter = townCenter.TownCenter;

        agent.Home = BuildingFactory.Instance.CreateBuilding(BuildingType.Home, agent.TownCenter);
    }
}
