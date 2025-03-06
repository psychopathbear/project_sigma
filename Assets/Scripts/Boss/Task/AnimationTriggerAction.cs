using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Animation Trigger", story: "Set animation trigger [Trigger] in [Animator] to: [TriggerState]", category: "Action", id: "67ebee5ac458936a088f18e456f7b5b2")]
public partial class AnimationTriggerAction : Action
{
    [SerializeReference] public BlackboardVariable<string> Trigger;
    [SerializeReference] public BlackboardVariable<Animator> Animator;
    [SerializeReference] public BlackboardVariable<bool> TriggerState;

    protected override Status OnStart()
    {
        if (Animator.Value == null)
        {
            LogFailure("No Animator set.");
            return Status.Failure;
        }

        if (TriggerState.Value)
        {
            Animator.Value.SetTrigger(Trigger.Value);
        }
        else
        {
            Animator.Value.ResetTrigger(Trigger.Value);
        }
        
        return Status.Success;
    }

}

