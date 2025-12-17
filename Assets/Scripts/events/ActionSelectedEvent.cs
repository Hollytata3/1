using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xianxiao
{
    public struct ActionSelectedEvent : IEvent
    {
        public ActionBase Action { get; }

        public ActionSelectedEvent(ActionBase action)
        {
            Action = action;
        }
    }

}
