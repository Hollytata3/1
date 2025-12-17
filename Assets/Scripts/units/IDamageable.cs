using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Xianxiao
{
    public interface IDamageable
    {
        public int MaxHealth { get; }
        public int CurrentHealth { get; }
        public Transform Transform { get; }
        public Owner Owner { get; }

        public void TakeDamage(int damage);
        public void Die();
    }

}
