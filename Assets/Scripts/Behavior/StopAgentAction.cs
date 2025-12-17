using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Xianxiao;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Stop Agent", story: "[Agent] stops moving .", category: "Action/Navigation", id: "093276707490d6a43f650dfd2d261bd3")]
public partial class StopAgentAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    protected override Status OnStart()
    {
        if (Agent.Value.TryGetComponent(out NavMeshAgent agent))
        {
            if(agent.TryGetComponent(out Animator animator))
            {
                animator.SetFloat(AnimationConstants.SPEED, 0);
            }
            agent.ResetPath();
            return Status.Success;
        }

        return Status.Failure;
    }
}


