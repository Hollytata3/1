using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Xianxiao
{

    public class BaseBuilding : AbstractCommandable
    {
        public int QueueSize => buildingQueue.Count;
        public AbstractUnitSO[] Queue => buildingQueue.ToArray();
        [field: SerializeField] public float CurrentQueueStartTime { get; private set; }
        [field: SerializeField] public AbstractUnitSO BuildingUnit { get; private set; }
        [field:SerializeField] public MeshRenderer MainRenderer { get; private set; }
        [SerializeField] private Material primaryMaterial;
        [SerializeField] private NavMeshObstacle navMeshObstacle;
        
        public delegate void QueueUpdatedEvent(AbstractUnitSO[] unitsInQueue);
        public event QueueUpdatedEvent OnQueueUpdated;
        public BuildingSO buildingSO;
        private List<AbstractUnitSO> buildingQueue = new(MAX_QUEUE_SIZE);
        private const int MAX_QUEUE_SIZE = 5;

        private void Awake()
        {
            buildingSO=UnitSO as BuildingSO;
        }
        protected override void Start()
        {
            base.Start();
            if (MainRenderer !=null)
            {
                MainRenderer.material = primaryMaterial;
            }
        }
        public void BuildUnit(AbstractUnitSO unit)
        {
            if (buildingQueue.Count == MAX_QUEUE_SIZE)
            {
                Debug.Log("建造上限为五");//队列已满时调用 BuildUnit
                return;
            }
            Bus<SupplyEvent>.Raise(new SupplyEvent(-unit.Cost.Minerals, unit.Cost.MineralsSO));
            Bus<SupplyEvent>.Raise(new SupplyEvent(-unit.Cost.Gas, unit.Cost.GasSO));

            buildingQueue.Add(unit);
            if (buildingQueue.Count == 1)
            {
                StartCoroutine(DoBuildUnits());
            }
            else
            {
                OnQueueUpdated?.Invoke(buildingQueue.ToArray());
            }
        }

        public void CancelBuildingUnit(int index)
        {
            if (index < 0 || index >= buildingQueue.Count)
            {
                Debug.Log("在队列范围之外取消建造单位");
                return;
            }
            AbstractUnitSO unitSO = buildingQueue[index];
            Bus<SupplyEvent>.Raise(new SupplyEvent(unitSO.Cost.Minerals, unitSO.Cost.MineralsSO));
            Bus<SupplyEvent>.Raise(new SupplyEvent(unitSO.Cost.Gas, unitSO.Cost.GasSO));
            buildingQueue.RemoveAt(index);
            if (index == 0)
            {
                StopAllCoroutines();

                if (buildingQueue.Count > 0)
                {
                    StartCoroutine(DoBuildUnits());
                }
                else
                {
                    OnQueueUpdated?.Invoke(buildingQueue.ToArray());
                }
            }
            else
            {
                OnQueueUpdated?.Invoke(buildingQueue.ToArray());
            }
        }
        public void ShowGhostVisuals()
        {
            MainRenderer.material = buildingSO.PlacementMaterial;
        }

        private IEnumerator DoBuildUnits()
        {
            while (buildingQueue.Count > 0)
            {
                BuildingUnit = buildingQueue[0];
                CurrentQueueStartTime = Time.time;
                OnQueueUpdated?.Invoke(buildingQueue.ToArray());

                yield return new WaitForSeconds(BuildingUnit.BuildTime);

                GameObject instance = Instantiate(BuildingUnit.Prefab, transform.position, Quaternion.identity);
                if (instance.TryGetComponent(out AbstractCommandable commandable))
                {
                    commandable.Owner = Owner;
                }

                buildingQueue.RemoveAt(0);
            }

            OnQueueUpdated?.Invoke(buildingQueue.ToArray());
        }
    }

}
