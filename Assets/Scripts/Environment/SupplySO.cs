using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Xianxiao
{
    [CreateAssetMenu(menuName = "Supply",fileName = "Supply",order =5 )]
    public class SupplySO:ScriptableObject
    {
        [field: SerializeField] public int MaxAmount { get; private set; } = 1500;
        [field: SerializeField] public int AmountPerGather { get; private set; } = 8;
        [field: SerializeField] public float BAseGatherTime { get; private set; } = 1.5f;
    }
}
