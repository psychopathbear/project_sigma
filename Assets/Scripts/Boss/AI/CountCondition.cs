using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Count", story: "Count to [repeatCount]", category: "Conditions", id: "dc86a39f3dac728fbe189c4f6f58fa00")]
public partial class CountCondition : Condition
{
    [SerializeReference] public BlackboardVariable<int> repeatCount;
    private int currentCount = 0; // Özel sayaç

    public override void OnStart()
    {
        Debug.Log("Count number is " + repeatCount.Value);
        currentCount = 0;
    }

    public override bool IsTrue()
    {
        if (currentCount < repeatCount.Value)
        {
            currentCount++;
            return true;
        }
        return false;
    }

}