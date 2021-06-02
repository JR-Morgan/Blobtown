using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentActor
{

    private IList<AgentBehaviour> behaviours;

    public AgentActor(IList<AgentBehaviour> behaviours)
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
