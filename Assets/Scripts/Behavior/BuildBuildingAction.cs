using System;
using Unity.Behavior;
using UnityEngine;
using Xianxiao;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Build Building", story: "[Self] builds [BuildingSO] at [TargetLocation] .", category: "Action/Units", id: "d88e2bf83dc6a9f8ac920b3a401a18a5")]
    public partial class BuildBuildingAction : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Self;
        [SerializeReference] public BlackboardVariable<BuildingSO> BuildingSO;
        [SerializeReference] public BlackboardVariable<Vector3> TargetLocation;
        [SerializeReference] public BlackboardVariable<BaseBuilding> BuildingUnderConstruction;

        private float startBuildTime;
        private BaseBuilding completedBuilding;
        private Renderer buildingRenderer;
        private Vector3 startPosition;
        private Vector3 endPosition;

        protected override Status OnStart()
        {
            if (!HasValidInputs()) return Status.Failure;

            startBuildTime = Time.time;
            GameObject building = GameObject.Instantiate(BuildingSO.Value.Prefab, TargetLocation, Quaternion.identity);

            if (!building.TryGetComponent(out completedBuilding)
                || completedBuilding.MainRenderer == null) return Status.Failure;

            buildingRenderer = completedBuilding.MainRenderer;

        if (Self.Value.TryGetComponent(out Worker worker))
        {
            completedBuilding.Owner = worker.Owner;
        }

        BuildingUnderConstruction.Value = completedBuilding;

            startPosition = TargetLocation.Value - Vector3.up * buildingRenderer.bounds.size.y;
            endPosition = TargetLocation.Value;
            buildingRenderer.transform.position = startPosition;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            float normalizedTime = (Time.time - startBuildTime) / BuildingSO.Value.BuildTime;

            buildingRenderer.transform.position = Vector3.Lerp(startPosition, endPosition, normalizedTime);

            return normalizedTime >= 1 ? Status.Success : Status.Running;
        }

        protected override void OnEnd()
        {
            if (CurrentStatus == Status.Success)
            {
                completedBuilding.enabled = true;
            }
        }

        private bool HasValidInputs()
        {
            return Self.Value != null
                && BuildingSO.Value != null
                && BuildingSO.Value.Prefab != null;
        }
    }





