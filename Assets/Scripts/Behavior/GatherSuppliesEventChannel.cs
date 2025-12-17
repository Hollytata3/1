using System;
using Unity.Behavior;
using UnityEngine;
using Xianxiao;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/GatherSuppliesEventChannel")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "GatherSuppliesEventChannel", message: "[Self] gathhers [Anount] [Supplies] .", category: "Events", id: "a68774b00c7e8c5d5edc129958d4d51e")]
public sealed partial class GatherSuppliesEventChannel : EventChannel<GameObject, int, SupplySO> { }

