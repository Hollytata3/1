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
    }

}
