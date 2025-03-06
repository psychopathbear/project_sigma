using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetHealth", story: "Set Health", category: "Action", id: "5f2e1cc4a784f9f239929e82ef238340")]
public partial class SetHealthAction : EnemyAction
{
    [SerializeReference] public BlackboardVariable<int> health;

    protected override Status OnUpdate()
    {
        enemyHealth.Value.currentHealth = health.Value;
        return Status.Success;
    }

}

