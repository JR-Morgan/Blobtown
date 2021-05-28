using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent
{

    private IList<AgentBehaviour> behaviours;

    public Agent(IList<AgentBehaviour> behaviours)
    {
        this.behaviours = behaviours;
    }

    public void Act()
    {
        var state = new BehaviourState();
        for (int i = 0; i <  behaviours.Count; i++)
        {
            if (behaviours[i].Invoke(state).shouldTerminate) break;
        }
    }

}
