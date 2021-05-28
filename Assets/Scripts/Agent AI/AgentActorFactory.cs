using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate BehaviourState AgentBehaviour(BehaviourState state);

public static class AgentActorFactory 
{
    
    public static AgentActor CreateActor()
    {
        return new AgentActor(CreateBehaviours());
    }

    public static List<AgentBehaviour> CreateBehaviours()
    {
        var behaviours = new List<AgentBehaviour>();

        //add behaviours depending on agent type?

        return behaviours;
    }


    #region Agent Types
    private static AgentBehaviour[] BasicAgent(AgentAI agent)
    {
        return new AgentBehaviour[]
        {
            GoHome(agent),
            Wait(agent)
        };
    }

    #endregion

    #region Helper Methods

    #endregion

    #region Behaviours
    private static AgentBehaviour Wait(AgentAI agent)
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            b.shouldTerminate = true;
            return b;
        }
        
       
    }

    private static AgentBehaviour GoHome(AgentAI agent)
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            b.shouldTerminate = true;
            agent.SetDestination(agent.home.transform.position);
            return b;
        }
    }

    #endregion

}

public class BehaviourState
{
    public bool shouldTerminate = false;
}