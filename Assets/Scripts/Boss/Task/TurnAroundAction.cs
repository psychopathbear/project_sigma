using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "TurnAround", story: "Turn Around", category: "Action", id: "82c56a7d6bd6fd5a7d66d7905d0c894a")]
public partial class TurnAroundAction : EnemyAction
{
    protected override Status OnUpdate()
    {
        var scale = body.Value.transform.localScale;
        scale.x *= -1;
        body.Value.transform.localScale = scale;
        return Status.Success;
    }

}

