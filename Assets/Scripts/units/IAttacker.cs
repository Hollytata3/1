using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Xianxiao
{
    public interface IAttacker
    {
            public Transform Transform { get; }
            public void Attack(IDamageable damageable);
            public bool CanAttack(IDamageable damageable); // 新增方法
    }

}

