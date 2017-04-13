using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentsGroup : AbstractAgent
{
    public AgentConfiguration[] agentsGroup;
     
    public override void agentAction()
    {
        for (int i = 0; i < agentsGroup.Length; i++)
        {
            agentsGroup[i].agent.generator = generator;
            if (agentsGroup[i].enable)
                for (int j = 0; j < agentsGroup[i].runs; j++)
                    agentsGroup[i].agent.agentAction();
        }
    }
}
