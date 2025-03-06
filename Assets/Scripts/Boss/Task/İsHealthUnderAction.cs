using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "isHealthUnder", story: "is Health Under", category: "Action", id: "b1e7bb569571d3b428c9b28572a38ecc")]
public partial class İsHealthUnderAction : EnemyAction
{
    protected override Status OnUpdate()
    {
        if(enemyHealth.Value.currentHealth < HealthThreshold.Value)
        {
            enemyCollider.Value.enabled = false;
            return Status.Success;
        }
        else
        {
            return Status.Failure;
        }
    }
}

