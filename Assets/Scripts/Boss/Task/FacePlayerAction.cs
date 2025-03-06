using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Scripts.AI
{
[Serializable, GeneratePropertyBag]
[NodeDescription(name: "FacePlayer", story: "Face [Player]", category: "Action", id: "fd0efef01bce3206cb44af31a4c09f43")]
public partial class FacePlayerAction : EnemyAction
{
    private float baseScaleX;

    protected override Status OnStart()
    {
        baseScaleX = Mathf.Abs(body.Value.transform.localScale.x);
        return base.OnStart();
    }

    protected override Status OnUpdate()
    {
        var scale = body.Value.transform.localScale;
        scale.x = player.Value.transform.position.x < body.Value.transform.position.x ? -baseScaleX : baseScaleX;
        body.Value.transform.localScale = scale;
        return Status.Success;
    }
}
}

