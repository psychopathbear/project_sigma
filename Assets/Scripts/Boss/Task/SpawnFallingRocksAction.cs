using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Scripts.Boss.Combat.Projectiles;
using DG.Tweening;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SpawnFallingRocks", story: "Spawn Falling Rocks", category: "Action", id: "01ebb7b1661410a94924e569c40901e1")]
public partial class SpawnFallingRocksAction : EnemyAction
{
    [SerializeReference] public BlackboardVariable<Collider2D> spawnAreaCollider;
    [SerializeReference] public BlackboardVariable<AbstractProjectile> rockPrefab;
    [SerializeReference] public BlackboardVariable<int> spawnCount;
    [SerializeReference] public BlackboardVariable<float> spawnInterval;

    protected override Status OnUpdate()
    {
        var sequence = DOTween.Sequence();
        for (int i = 0; i < spawnCount.Value; i++)
        {
            sequence.AppendCallback(SpawnRock);
            sequence.AppendInterval(spawnInterval.Value);
        }
        return Status.Success;
    }

    private void SpawnRock()
    {
        var randomX = UnityEngine.Random.Range(spawnAreaCollider.Value.bounds.min.x, spawnAreaCollider.Value.bounds.max.x);
        var rock = UnityEngine.Object.Instantiate(rockPrefab.Value, new Vector3(randomX, spawnAreaCollider.Value.bounds.min.y - 1f), Quaternion.identity);
        rock.SetForce(Vector2.zero);
    }
}

