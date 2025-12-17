using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set Agent Avoidance", story: "Set [Agent] avoidance quality to [AvoidanceQuality] .", category: "Action/Navigation", id: "16d0a11e3e5d37a52b680f97786b10ed")]
public partial class SetAgentAvoidanceAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<int> AvoidanceQuality;

    protected override Status OnStart()
    {
        if (!Agent.Value.TryGetComponent(out NavMeshAgent agent) || AvoidanceQuality > 4 || AvoidanceQuality < 0)
        {
            return Status.Failure;
        }

        agent.obstacleAvoidanceType = (ObstacleAvoidanceType)AvoidanceQuality.Value;

        return Status.Success;
    }
}



