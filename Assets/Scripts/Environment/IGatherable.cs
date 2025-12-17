using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xianxiao
{
    public interface IGatherable
    {
        public SupplySO Supply { get; }
        public int Amount { get; }
        public bool IsBusy { get; }
        public bool BeginGather();
        public int EndGather();
        public void AbortGather();
    }
}
