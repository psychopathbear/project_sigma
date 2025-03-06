using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SpawnMaggot", story: "Spawn Maggot", category: "Action", id: "a78d5330eabaecca0463091bf71511b3")]
public partial class SpawnMaggotAction : EnemyAction
{
    [SerializeReference] public BlackboardVariable<GameObject> maggotPrefab;
    [SerializeReference] public BlackboardVariable<Transform> maggotTransform;

    private EnemyHealth maggot;

    protected override Status OnStart()
    {
        if (maggotPrefab.Value == null || maggotTransform.Value == null)
        {
            Debug.LogError("Maggot prefab or transform is not set.");
            return Status.Failure;
        }

        maggot = UnityEngine.Object.Instantiate(maggotPrefab.Value, maggotTransform.Value).GetComponent<EnemyHealth>();
        if (maggot == null)
        {
            Debug.LogError("Failed to instantiate maggot or get EnemyHealth component.");
            return Status.Failure;
        }

        maggot.transform.localPosition = Vector3.zero;
        //hazardCollider.Value.SetActive(false);
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (maggot == null)
        {
            Debug.LogError("Maggot is null in OnUpdate.");
            return Status.Failure;
        }

        if (maggot.currentHealth > 0)
        {
            return Status.Running;
        }
        //hazardCollider.Value.SetActive(true);
        return Status.Success;
    }
}

