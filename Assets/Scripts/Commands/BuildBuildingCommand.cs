using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Xianxiao
{
    [CreateAssetMenu(fileName ="Build Building",menuName = "Units/Commands/Build Building")]
    public class BuildBuildingCommand : BaseCommand
    {
        [field: SerializeField] public BuildingSO building { get; private set; }
        [field: SerializeField] public BuildingRestrictionSO[] Restrictions { get; private set; }
        public override bool CanHandle(CommandContext context)
        {
            return HasEnoughSupplies() && AllRestrictionsPass(context.Hit.point);

        }

        public override void Handle(CommandContext context)
        {
            IBuildingBuilder builder=(IBuildingBuilder)context.Commandable;
            builder.Build(building, context.Hit.point);
        }
        private bool AllRestrictionsPass(Vector3 point) =>
          Restrictions.Length == 0 || Restrictions.All(restriction => restriction.CanPlace(point));
        public override bool IsLocked(CommandContext context) => !HasEnoughSupplies();
        private bool HasEnoughSupplies() => building.Cost.Minerals <= Supplies.Minerals && building.Cost.Gas <= Supplies.Gas;
    }
}
