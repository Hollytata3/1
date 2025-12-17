using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xianxiao
{
    public struct CommandSelectedEvent : IEvent
    {
        public BaseCommand Command { get; }

        public CommandSelectedEvent(BaseCommand command)
        {
            Command = command;
        }
    }

}
