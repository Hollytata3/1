using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Xianxiao;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Move to Target GameObject", story: "[Agent] moves to [TargetGameObject] .", category: "Action/Navigation", id: "e4eaaec0b30a3b01dd58d962c5abc876")]
public partial class MoveToTargetGameObjectAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;

    private NavMeshAgent agent;
    private Animator animator;

    protected override Status OnStart()
    {
        if (!Agent.Value.TryGetComponent(out agent))
        {
            return Status.Failure;
        }
        agent.TryGetComponent(out animator);

        Vector3 targetPosition = TargetGameObject.Value.transform.position;
        agent.SetDestination(targetPosition);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (animator != null)
        {
            animator.SetFloat(AnimationConstants.SPEED, agent.velocity.magnitude);
        }
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
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






