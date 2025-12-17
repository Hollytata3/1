using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Xianxiao;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Move to Target Location", story: "[Agent] moves to [TargetLocation] .", category: "Action/Navigation", id: "13518029487b05c760b5ce6c62f6b391")]
public partial class MoveToTargetLocationAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Vector3> TargetLocation;

    private NavMeshAgent agent;
    private Animator animator;

    protected override Status OnStart()
    {
        if (!Agent.Value.TryGetComponent(out agent))
        {
            return Status.Failure;
        }
        Agent.Value.TryGetComponent(out animator);

        if (Vector3.Distance(agent.transform.position, TargetLocation.Value) <= agent.stoppingDistance)
        {
            return Status.Success;
        }

        agent.SetDestination(TargetLocation.Value);

        return Status.Running;
    }
    

    protected override Status OnUpdate()
    {
        if (animator != null)
        {
            animator.SetFloat(AnimationConstants.SPEED,agent.velocity.magnitude);
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            return Status.Success;
        }

        return Status.Running;
    }
    protected override void OnEnd()
    {
        if (animator != null)
        {
            animator.SetFloat(AnimationConstants.SPEED, 0);
        }
    }
}



