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
            //DropOffScout(agent),
            //SurveyTiles()
            //MoveToAdjTile()
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


    private static AgentBehaviour BuilderComplete(AgentAI agent)
    {
        return Action;
        BehaviourState Action(BehaviourState b)
        {
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

    private static AgentBehaviour MineTarget(AgentAI agent) //agent works on the nearest resource slowly increasing the progress
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            foreach (Tile t in agent.AdjacentTiles)
            {
                if (t.TileType == TileType.Ore)
                {
                    agent.Inventory.AddResource(t.TileData.resourceType, t.TileData.amount);

                    t.TileType = TileType.Default;

                    b.shouldTerminate = true;
                    break;
                }
            }


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

    private static AgentBehaviour TestBuildingSpot(AgentAI agent) //when at the correct plot, check it for suitability
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            return b;
        }
    }

    private static AgentBehaviour StartBuildingHome(AgentAI agent) //when at the correct plot and it has been checked. Place the home plot
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            return b;
        }
    }

    private static AgentBehaviour BuildHome(AgentAI agent) //Continue the home building, increasing it's progress bar
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {

            if (agent.Home != null && (agent.Home.Inventory.HasResource(ResourceType.Ore, 5) || agent.Home.Inventory.HasResource(ResourceType.Wood, 5)))
            {
                agent.Inventory.SubtractResource(ResourceType.Ore, 5);
                agent.Inventory.SubtractResource(ResourceType.Wood, 5);

                agent.Home = BuildingFactory.Instance.CreateBuilding(BuildingType.Home, agent.TownCenter);

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
            if (!(agent.Tile.HasResource && agent.Tile.TileData.resourceType == resourceType))
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
    Builder,
}