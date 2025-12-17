using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Xianxiao;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack Target", story: "[Self] attacks [Target] until it dies .", category: "Action", id: "5a2861849d37f1c44829b9650d2ad5a3")]
public partial class AttackTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<AttackConfigSO> AttackConfig;

    private NavMeshAgent navMeshAgent;
    private AbstractUnit unit;
    private Transform selfTransform;
    private Animator animator;

    private IDamageable targetDamageable;
    private Transform targetTransform;

    private float lastAttackTime;

    protected override Status OnStart()
    {
        if (!HasValidInputs()) return Status.Failure;

        selfTransform = Self.Value.transform;
        navMeshAgent = selfTransform.GetComponent<NavMeshAgent>();
        animator = selfTransform.GetComponent<Animator>();
        unit=selfTransform.GetComponent<AbstractUnit>();

        targetTransform = Target.Value.transform;
        targetDamageable = Target.Value.GetComponent<IDamageable>();
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Target.Value == null || targetDamageable.CurrentHealth == 0) return Status.Success;
         // 添加Owner检查
    if (targetDamageable.Owner == unit.Owner)
    {
        // 如果是同阵营单位，停止攻击
        navMeshAgent.isStopped = true;
        if (animator != null)
        {
            animator.SetBool(AnimationConstants.ATTACK, false);
        }
        return Status.Success;
    }
        if (animator != null)
        {
            animator.SetFloat(AnimationConstants.SPEED, navMeshAgent.velocity.magnitude);
        }

        if (Vector3.Distance(targetTransform.position, selfTransform.position) >= AttackConfig.Value.AttackRange)
        {
            navMeshAgent.SetDestination(targetTransform.position);
            navMeshAgent.isStopped = false;
            if (animator != null)
            {
                animator.SetBool(AnimationConstants.ATTACK, false);
            }
            return Status.Running;
        }

        navMeshAgent.isStopped = true;
        Quaternion lookRotation=Quaternion.LookRotation(
            (targetTransform.position-selfTransform.position).normalized,
            Vector3.up
            );
        selfTransform.rotation = Quaternion.Euler(
            selfTransform.rotation.eulerAngles.x,
            lookRotation.eulerAngles.y,
            selfTransform.rotation.eulerAngles.z);
        if (animator != null)
        {
            animator.SetBool(AnimationConstants.ATTACK, true);
        }

        if (Time.time >= lastAttackTime + AttackConfig.Value.AttackDelay)
        {
            lastAttackTime = Time.time;
            if (unit.AttackingParticleSystem != null)
            {
                unit.AttackingParticleSystem.Play();//枪口==>粒子发射一次
            }
            targetDamageable.TakeDamage(AttackConfig.Value.Damage);
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (animator != null)
        {
            animator.SetBool(AnimationConstants.ATTACK, false);
        }
    }

    private bool HasValidInputs() => Self.Value != null && Self.Value.TryGetComponent(out NavMeshAgent _)
        &&Self.Value.TryGetComponent(out AbstractUnit _)
        && Target.Value != null && Target.Value.TryGetComponent(out IDamageable _)
        && AttackConfig.Value != null;
}
