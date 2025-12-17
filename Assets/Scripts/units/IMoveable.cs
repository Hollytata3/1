using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Xianxiao
{
    public interface IMoveable
    {
        void MoveTo(Vector3 position);
        void Stop();
    }

}
