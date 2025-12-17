using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Sample Position", story: "Set [TargetLocation] to the closest point on the NavMesh to [Target] .", category: "Action/Navigation", id: "2b88b1038fca3677e657c9b137cc4142")]
public partial class SamplePositionAction : Action
{
    [SerializeReference] public BlackboardVariable<Vector3> TargetLocation;
    [SerializeReference] public BlackboardVariable<NavMeshAgent> Target;
    [SerializeReference] public BlackboardVariable<float> SearchRadius = new(5);

    protected override Status OnStart()
    {
        if (Target.Value == null) return Status.Failure;

        NavMeshQueryFilter navMeshQueryFilter = new()
        {
            agentTypeID = Target.Value.agentTypeID,
            areaMask = Target.Value.areaMask
        };

        if (NavMesh.SamplePosition(Target.Value.transform.position, out NavMeshHit hit, SearchRadius, navMeshQueryFilter))
        {
            TargetLocation.Value = hit.position;
            return Status.Success;
        }

        return Status.Failure;
    }
}


