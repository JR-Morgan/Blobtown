using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate BehaviourState AgentBehaviour(BehaviourState state);

public class AgentFactory 
{
    
    public static Agent CreateAgent()
    {
        return new Agent(CreateBehaviours());
    }

    public static List<AgentBehaviour> CreateBehaviours()
    {
        var behaviours = new List<AgentBehaviour>();

        //add behaviours depending on agent type?

        return behaviours;
    }


    #region Agent Types
    private static AgentBehaviour[] BasicAgent(Agent agent)
    {
        return new AgentBehaviour[]
        {
            Wait()
        };
    }

    #endregion

    #region Helper Methods

    #endregion

    #region Behaviours
    private static AgentBehaviour Wait()
    {
        return Action;

        BehaviourState Action(BehaviourState b)
        {
            b.shouldTerminate = true;
            return b;
        }
        
       
    }

    #endregion

}

public class BehaviourState
{
    public bool shouldTerminate = false;
}