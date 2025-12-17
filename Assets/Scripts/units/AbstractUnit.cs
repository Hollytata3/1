using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Xianxiao;

namespace Xianxiao
{
    [RequireComponent(typeof(NavMeshAgent), typeof(BehaviorGraphAgent))]
    public abstract class AbstractUnit : AbstractCommandable, IMoveable, IAttacker
    {
        public float AgentRadius => agent.radius;
        [field:SerializeField]public ParticleSystem AttackingParticleSystem {  get; private set; }
        [SerializeField] private DamageableSensor DamageableSensor;
        private NavMeshAgent agent;
        protected BehaviorGraphAgent graphAgent;
        protected UnitSO unitSO;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            graphAgent = GetComponent<BehaviorGraphAgent>();
            unitSO = UnitSO as UnitSO;
            graphAgent.SetVariableValue("Command", UnitCommands.Stop);
            graphAgent.SetVariableValue("AttackConfig", unitSO.AttackConfig);
        }

        protected override void Start()
        {
            base.Start();
            Bus<UnitSpawnEvent>.Raise(new UnitSpawnEvent(this));
            if (DamageableSensor != null)
            {
                DamageableSensor.OnUnitEnter += HandleUnitEnterOrExit;
                DamageableSensor.OnUnitExit += HandleUnitEnterOrExit;
                DamageableSensor.Owner = Owner;
                DamageableSensor.SetupFrom(unitSO.AttackConfig);

            }

        }


        public void MoveTo(Vector3 position)
        {
            graphAgent.SetVariableValue("TargetLocation", position);
            graphAgent.SetVariableValue("Command", UnitCommands.Move);
        }

        public void Stop()
        {
            graphAgent.SetVariableValue("Command", UnitCommands.Stop);
        }
        public void Attack(IDamageable damageable)
        {
            graphAgent.SetVariableValue("TargetGameObject", damageable.Transform.gameObject);
            graphAgent.SetVariableValue("Command", UnitCommands.Attack);

        }

        private void HandleUnitEnterOrExit(IDamageable damageable)
        {
            List<GameObject> nearbyEnemies = DamageableSensor.Damageables
                .ConvertAll(damageable => damageable.Transform.gameObject);
            nearbyEnemies.Sort(new ClosestGameObjectComparer(transform.position));

            graphAgent.SetVariableValue("NearbyEnemies", nearbyEnemies);
        }
        public bool CanAttack(IDamageable damageable)
        {
            return damageable != null && damageable.Owner != Owner;
        }


        private void OnDestroy()
        {
            Bus<UnitDeathEvent>.Raise(new UnitDeathEvent(this));
        }
    }
}



