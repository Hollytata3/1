using System;
using Unity.Behavior;
using Unity.VisualScripting;
using UnityEngine;

namespace Xianxiao
{
    public class Worker : AbstractUnit,IBuildingBuilder
    {
        [SerializeField] private BaseCommand CancelBuildingCommand;
        protected override void Start()
        {

            base.Start();
            if (graphAgent.GetVariable("GatherSuppliesEvent", out BlackboardVariable<GatherSuppliesEventChannel> eventChannelVariable))
            {
                eventChannelVariable.Value.Event += HandleGatherSupplies;
            }
        }

        public void Gather(GatherablesSupply supply)
        {
            graphAgent.SetVariableValue("Supply", supply);
            graphAgent.SetVariableValue("TargetLocation", supply.transform.position);
            graphAgent.SetVariableValue("Command", UnitCommands.Gather);
        }
        public GameObject Build(BuildingSO building, Vector3 targetLocation)
        {
            GameObject instance= Instantiate(building.Prefab, targetLocation, Quaternion.identity);
            if(instance.TryGetComponent(out BaseBuilding basebuilding))
            {
                basebuilding.ShowGhostVisuals();
                basebuilding.Owner = this.Owner;
            }
            else
            {
             Debug.LogError("该建筑预制件没有建筑");
                return null;
            }
            graphAgent.SetVariableValue("BuildingSO", building);
            graphAgent.SetVariableValue("TargetLocation", targetLocation);
            graphAgent.SetVariableValue("Ghost", instance);
            graphAgent.SetVariableValue("Command", UnitCommands.BuildBuilding);
            SetCommandOverrides(new BaseCommand[] { CancelBuildingCommand });
            Bus<UnitSelectedEvent>.Raise(new UnitSelectedEvent(this));
            Bus<SupplyEvent>.Raise(new SupplyEvent(-building.Cost.Minerals, building.Cost.MineralsSO));
            Bus<SupplyEvent>.Raise(new SupplyEvent(-building.Cost.Gas, building.Cost.GasSO));


            return instance;
        }
        public void CancelBuilding()
        {
            if (graphAgent.GetVariable("Ghost", out BlackboardVariable<GameObject> ghostVariable)
                && ghostVariable.Value != null)
            {
                Destroy(ghostVariable.Value);
            }
            if (graphAgent.GetVariable("BuildingUnderConstruction", out BlackboardVariable<BaseBuilding> buildingVariable)
                && buildingVariable.Value != null)
            {
                Destroy(buildingVariable.Value.gameObject);
                BuildingSO buildingSO = buildingVariable.Value.buildingSO;
                Bus<SupplyEvent>.Raise(new SupplyEvent(
                    Mathf.FloorToInt(0.5f * buildingSO.Cost.Minerals),
                    buildingSO.Cost.MineralsSO
                ));
                Bus<SupplyEvent>.Raise(new SupplyEvent(
                    Mathf.FloorToInt(0.75f * buildingSO.Cost.Gas),
                    buildingSO.Cost.GasSO
                ));

            }

            SetCommandOverrides(Array.Empty<BaseCommand>());
            Stop();
        }

        private void HandleGatherSupplies(GameObject self, int amount, SupplySO supply)
        {
            Bus<SupplyEvent>.Raise(new SupplyEvent(amount, supply));
        }
    }
}

