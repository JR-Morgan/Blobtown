using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate BehaviourState AgentBehaviour(BehaviourState state);

public static class AgentActorFactory 
{
    
    public static AgentActor CreateActor(AgentAI agent, AgentType agentType = default)
    {
        return new AgentActor(CreateBehaviours(agent, agentType));
    }

    public static AgentBehaviour[] CreateBehaviours(AgentAI agent, AgentType agentType)
    {
        return agentType switch
        {
            AgentType.Farmer => Farmer(agent),
            AgentType.Miner => ResourceGathererAgent(agent, ResourceType.Ore, 3f),
            AgentType.WoodCutter => ResourceGathererAgent(agent, ResourceType.Wood, 3f),
            AgentType.Scout => Scout(agent),
            //AgentType.Builder => Builder(agent),
            _ => throw new System.NotImplementedException($"No behaviour exists for {agentType}"),
        };
    }


    #region Agent Types

    private static AgentBehaviour[] JoblessAgent(AgentAI agent)
    {
        return new AgentBehaviour[]
        {
            //GetJobFromMyTownHall
            //BuildTownHall
        };
    }


    private static AgentBehaviour[] ResourceGathererAgent(AgentAI agent, ResourceType resourceType, float progressRequired)
    {
        return new AgentBehaviour[]
        {
            Build(agent),
            DropOffResources(agent),
            HarvestResource(agent, resourceType, progressRequired),
            MoveToResource(agent, resourceType),
            MoveRandomly(agent),
        };
    }

    private static AgentBehaviour[] Scout(AgentAI agent)
    {
        return new AgentBehaviour[]
        {
            Build(agent),
            //DropOffScout(agent),
            SurveyTiles(agent, 7f),
            MoveToRandomTileInRadius(agent, 10f),
            MoveRandomly(agent),
        };
    }

    //private static AgentBehaviour[] Builder(AgentAI agent)
    //{
    //    return new AgentBehaviour[]
    //    {
    //
    //        BuilderComplete(agent),
    //        //BuildHome
    //        //TestBuildingPlot
    //        //StartBuilding
    //        //MoveToBuildingPlot
    //        
    //    };
    //}

    private static AgentBehaviour[] Farmer(AgentAI agent)
    {
        return new AgentBehaviour[]
        {
            Build(agent),
            DropOffResources(agent),
            TakeResourceFromBuilding(agent, BuildingType.Farm, ResourceType.Food, 1),
            MoveToBuidlingType(agent, BuildingType.Farm),
            MoveRandomly(agent),
        };
    }

    #endregion

    #region Helper Methods

    #endregion

    #region Behaviours

    private static AgentBehaviour MoveToBuidlingType(AgentAI agent, BuildingType buildingType)
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            List<Building> targetBuildings = BuildingFactory.Instance.Buildings[buildingType];
            if(targetBuildings.Count > 0)
            {
                agent.Goal = targetBuildings[Random.Range(0, targetBuildings.Count - 1)].Position;
                b.shouldTerminate = true;
            }
            return b;
        }
    }

    private static AgentBehaviour TakeResourceFromBuilding(AgentAI agent, BuildingType buildingType, ResourceType resource, int amount)
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            if (agent.Tile.HasBuilding && agent.Tile.Building.BuildingType == buildingType)
            {
                Building building = agent.Tile.Building;
                if (building.Inventory.SubtractResource(resource, amount))
                {
                    agent.Inventory.AddResource(resource, amount);
                }
                b.shouldTerminate = true;

            }
            return b;
        }
    }

    private static AgentBehaviour DropOffResources(AgentAI agent) //Drop off resources at town Centre and get new job
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {

            if(!agent.Inventory.IsEmpty)
            {
                Tile townCenter = null;

                if (agent.Tile.Building == agent.TownCenter.Building)
                {
                    townCenter = agent.Tile;
                }
                else
                {
                    townCenter = agent.AdjacentTiles.Find(t => t.Building == agent.TownCenter.Building);
                }

                if (townCenter != null)
                {
                    agent.TownCenter.Building.Inventory.AddResources(agent.Inventory.Contents);
                    agent.Inventory.Clear();
                } 
                else
                {
                    b.shouldTerminate = true;

                    agent.Goal = agent.TownCenter.Building.Position;
                }

            }

            return b;
        }
    }

    private static AgentBehaviour MoveToRandomTileInRadius(AgentAI agent, float radius)
    {
        return Action;
        BehaviourState Action(BehaviourState b)
        {
            List<Tile> candidateGoals = agent.Tile.Grid.TilesInCircle(agent.Tile, radius).Where(t => t.Discovered).ToList();
            if(candidateGoals.Count > 0)
            {
                agent.Goal = candidateGoals[Random.Range(0, candidateGoals.Count)];
                b.shouldTerminate = true;
            }
            return b;
        }
    }


    private static AgentBehaviour SurveyTiles(AgentAI agent, float radius)
    {
        return Action;
        BehaviourState Action(BehaviourState b)
        {
            foreach (Tile t in agent.Tile.Grid.TilesInCircle(agent.Tile, radius))
            {
                t.Discovered = true;
            }
            return b;
        }
    }


    private static AgentBehaviour MoveRandomly(AgentAI agent)
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            var adjTiles = agent.AdjacentTiles;
            agent.Goal = adjTiles[Random.Range(0, adjTiles.Count - 1)];
            b.shouldTerminate = true;
            
            return b;
        }
    }

    private static AgentBehaviour HarvestResource(AgentAI agent, ResourceType resourceType, float progressRequired)
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            if(agent.Progress == 0)
            {
                agent.ProgressCanvas.gameObject.SetActive(true);
            }
            agent.Progress += Time.deltaTime;
            agent.ProgressBar.fillAmount = agent.Progress / progressRequired;

            if (agent.Progress >= progressRequired)
            {
                if (agent.Tile.HasResource && agent.Tile.TileData.resourceType == resourceType)
                {
                    agent.Inventory.AddResource(agent.Tile.TileData.resourceType, agent.Tile.TileData.amount);

                    agent.Tile.TileType = TileType.Default;

                    b.shouldTerminate = true;
                }
            }

            return b;
        }
    }


    private static AgentBehaviour Build(AgentAI agent)
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            const int ORE_AMOUNT = 2;
            const int WOOD_AMOUNT = 2;
            Building tc = agent.TownCenter.Building;
            if (tc.Inventory.HasResource(ResourceType.Ore, ORE_AMOUNT) && tc.Inventory.HasResource(ResourceType.Wood, WOOD_AMOUNT))
            {
                tc.Inventory.SubtractResource(ResourceType.Ore, ORE_AMOUNT);
                tc.Inventory.SubtractResource(ResourceType.Wood, WOOD_AMOUNT);

                //agent.Home = BuildingFactory.Instance.CreateBuilding(BuildingType.Home, agent.TownCenter);
                BuildingFactory.Instance.CreateBuilding(BuildingType.Farm, agent.TownCenter);
                AgentFactory.Instance.PlaceAgent(agent.TownCenter.Building.Position);

                b.shouldTerminate = true;
            }

            return b;
        }
    }

    private static AgentBehaviour MoveToResource(AgentAI agent, ResourceType resourceType) //go to the resource indicated by the town centre
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            if (!(agent.Tile.TileData != null && agent.Tile.TileData.resourceType == resourceType))
            {
                var resources = agent.TownCenter.KnownResources[resourceType];
                if (resources.Count > 0)
                {
                    agent.Goal = resources[Random.Range(0, resources.Count)];
                    b.shouldTerminate = true;
                }
            }
            else
            {
                b.shouldTerminate = true;
            }
            return b;
        }
    }

    #endregion

}

public class BehaviourState
{
    public bool shouldTerminate = false;
}

public enum AgentType
{
    Farmer,
    Miner,
    WoodCutter,
    Scout,
}