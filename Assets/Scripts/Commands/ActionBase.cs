using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Xianxiao
{
    public abstract class ActionBase : ScriptableObject, IComand
    {
        [field: SerializeField]public Sprite Icon { get; private set; }
        [field:Range(0,8)][field: SerializeField] public int Slot { get; private set; }
        [field: SerializeField] public bool RequiresClickToActivate { get; private set; } = true;
        [field: SerializeField] public GameObject GhostPrefab { get; private set; }
        public abstract bool CanHandle(CommandContext context);
        public abstract void Handle(CommandContext context);

    }
}
