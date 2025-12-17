using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Set NavMeshAgent Enabled", story: "[Self] sets NavMeshAgent active status to [Active] .", category: "Action/Navigation", id: "c3e17540d952c70632d40a4d62cba1a3")]
public partial class SetNavMeshAgentEnabledAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<bool> Active;

    protected override Status OnStart()
    {
        if (Self.Value == null || !Self.Value.TryGetComponent(out NavMeshAgent agent)) return Status.Failure;

        agent.enabled = Active;

        return Status.Success;
    }
}


