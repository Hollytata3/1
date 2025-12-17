using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Xianxiao;

namespace Xianxiao
{
    [CreateAssetMenu(fileName = "Gather Action", menuName = "Units/Commands/Gather",order=105)]
    public class GatherCommand : BaseCommand
    {
        public override bool CanHandle(CommandContext context)
        {
            return context.Commandable is Worker
                && context.Hit.collider != null
                && context.Hit.collider.TryGetComponent(out GatherablesSupply _);
        }
        public override bool IsLocked(CommandContext context) => false;

        public override void Handle(CommandContext context)
        {
            Worker worker = context.Commandable as Worker;
            worker.Gather(context.Hit.collider.GetComponent<GatherablesSupply>());
        }
    }
}

