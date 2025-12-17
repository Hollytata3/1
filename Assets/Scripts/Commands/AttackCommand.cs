using UnityEngine;
using Xianxiao;

namespace Xianxiao
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Units/Commands/Attack", order = 99)]
    public class AttackCommand : BaseCommand
    {
        public override bool CanHandle(CommandContext context)
        {
            // 添加Owner检查
            if (context.Hit.collider.TryGetComponent(out IDamageable damageable))
            {
                if (context.Commandable is AbstractCommandable commandable
                    && damageable.Owner == commandable.Owner)
                {
                    return false;
                }
            }
                return context.Commandable is IAttacker
                && context.Hit.collider != null && context.Hit.collider.TryGetComponent(out IDamageable _);
        }

        public override void Handle(CommandContext context)
        {
            IAttacker attacker = context.Commandable as IAttacker;
            attacker.Attack(context.Hit.collider.GetComponent<IDamageable>());
        }

        public override bool IsLocked(CommandContext context) => false;
    }
}

