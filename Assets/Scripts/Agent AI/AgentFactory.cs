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

}

public class BehaviourState
{
    public bool shouldTerminate = false;
}