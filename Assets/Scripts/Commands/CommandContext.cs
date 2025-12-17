using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Xianxiao
{
    public struct CommandContext
    {
        public AbstractCommandable Commandable { get; private set; }
        public RaycastHit Hit { get; private set; }
        public int UnitIndex { get; private set; }

        public CommandContext(AbstractCommandable commandable, RaycastHit hit, int unitIndex = 0)
        {
            Commandable = commandable;
            Hit = hit;
            UnitIndex = unitIndex;
        }
    }

}
