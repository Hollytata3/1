using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Xianxiao
{
        [RequireComponent(typeof(SphereCollider))]
        public class DamageableSensor : MonoBehaviour
        {
            public List<IDamageable> Damageables => damageables.ToList();
            [field: SerializeField] public Owner Owner { get; set; }

            public delegate void UnitDetectionEvent(IDamageable damageable);
            public event UnitDetectionEvent OnUnitEnter;
            public event UnitDetectionEvent OnUnitExit;
            private new SphereCollider collider;
            private HashSet<IDamageable> damageables = new();

        private void Awake()
        {
            collider = GetComponent<SphereCollider>();
        }


        private void OnTriggerExit(Collider collider)
            {
                if (collider.TryGetComponent(out IDamageable damageable) && damageables.Remove(damageable))
                {
                    OnUnitExit?.Invoke(damageable);
                }
            }
        public void SetupFrom(AttackConfigSO attackConfig)
        {
            collider.radius = attackConfig.AttackRange;
        }

    }
}

