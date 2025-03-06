using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Diagnostics;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "GoToNextStage", story: "Go To Next Stage", category: "Action", id: "5df61d0aedf38d7552906527cd499cc8")]
public partial class GoToNextStageAction : EnemyAction
{
    protected override Status OnStart()
    {
        currentStage.Value ++;
        UnityEngine.Debug.Log("Current stage: " + currentStage);
        enemyCollider.Value.enabled = true;
        return Status.Success;
    }

}

