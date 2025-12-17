using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Xianxiao;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Find Closest Command Post", story: "[Unit] finds nearest [CommandPost] .", category: "Action/Units", id: "1aef9cf1d02cf6ef2003279335154a5e")]
public partial class FindClosestCommandPostAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Unit;
    [SerializeReference] public BlackboardVariable<GameObject> CommandPost;
    [SerializeReference] public BlackboardVariable<float> SearchRadius = new(10);
    [SerializeReference] public BlackboardVariable<BuildingSO> CommandPostBuilding;

    protected override Status OnStart()
    {
        Collider[] colliders = Physics.OverlapSphere(
            Unit.Value.transform.position,
            SearchRadius.Value,
            LayerMask.GetMask("Buildings"));

        List<BaseBuilding> nearbyCommandPosts = new();

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out BaseBuilding building)
                && building.UnitSO == CommandPostBuilding.Value)
            {
                nearbyCommandPosts.Add(building);
            }
        }

        if (nearbyCommandPosts.Count == 0)
        {
            CommandPost.Value = null;
            return Status.Failure;
        }
        BaseBuilding closest = nearbyCommandPosts
            .OrderBy(b => Vector3.Distance(Unit.Value.transform.position, b.transform.position))
            .First();

        CommandPost.Value = closest.gameObject;
        return Status.Success;
    }
}



