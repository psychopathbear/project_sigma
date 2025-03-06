using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Core;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FreezeTime", story: "Freeze Time", category: "Action", id: "ce549b1b6f55246778f6bb576e5568b3")]
public partial class FreezeTimeAction : Action
{
    [SerializeReference] public BlackboardVariable<float> freezeTime;

    protected override Status OnUpdate()
    {
        GameManager.Instance.FreezeTime(freezeTime.Value);
        return Status.Success;
    }
}

