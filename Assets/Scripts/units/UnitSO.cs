using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Xianxiao
{
    [CreateAssetMenu(fileName = "Unit", menuName = "Units/Unit")]
    public class UnitSO:AbstractUnitSO
    {
        [field: SerializeField] public AttackConfigSO AttackConfig { get; private set; }
    }
}
