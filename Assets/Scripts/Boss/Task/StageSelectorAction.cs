using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StageSelector", story: "Stage [stageInt] Selected", category: "Action", id: "091a1055addf2cfcf7d51e55504c4d5d")]
public partial class StageSelectorAction : EnemyAction
{
    [SerializeReference] public BlackboardVariable<int> StageInt;

    protected override Status OnUpdate()
    {
        if(currentStage.Value >= StageInt.Value && enemyHealth.Value.currentHealth >= HealthThreshold.Value)
        {
            Debug.Log("Stage " + StageInt.Value + " Selected " + "Current stage: " + currentStage);
            return Status.Success;
        }
        else
        {
            return Status.Failure;
        }
    }
}

