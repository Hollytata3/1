using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Xianxiao
{
    [CreateAssetMenu(fileName = "Building", menuName = "Buildings/Building")]
    public class BuildingSO:AbstractUnitSO
    {
        [field: SerializeField] public Material PlacementMaterial { get; private set; }
        [field: SerializeField] public float HealRange { get; private set; } = 10f;
        [field: SerializeField] public float HealAmount { get; private set; } = 5f;
        [field: SerializeField] public float HealInterval { get; private set; } = 2f;
    }

}
