using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Xianxiao
{
    [CreateAssetMenu(fileName = "Cancel Building", menuName = "Units/Commands/Cancel Building")]
    public class CancelBuildingCommand : BaseCommand
    {
        public override bool CanHandle(CommandContext context)
        {
            return context.Commandable is IBuildingBuilder;
        }

        public override void Handle(CommandContext context)
        {
            IBuildingBuilder buildingBuilder = context.Commandable as IBuildingBuilder;
            buildingBuilder.CancelBuilding();
        }
        public override bool IsLocked(CommandContext context) => false;
    }
}

