using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections.Generic;
using Scripts.Boss.Combat;
using Scripts.Boss.Combat.Projectiles;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Shoot", story: "Shoot", category: "Action", id: "168d8e7d9a6e834e4b2f8d2b2cfc8651")]
public partial class ShootAction : EnemyAction
{
    //[SerializeReference] public BlackboardVariable<Weapon> weapon;
    [SerializeReference] public BlackboardVariable<AbstractProjectile> projectilePrefab;
    [SerializeReference] public BlackboardVariable<Transform> weaponTransform;
    [SerializeReference] public BlackboardVariable<bool> shakeCamera;
    [SerializeReference] public BlackboardVariable<float> horizontalForce;
    [SerializeReference] public BlackboardVariable<float> verticalForce;


    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
            var projectile = UnityEngine.Object.Instantiate(projectilePrefab.Value, weaponTransform.Value.position, Quaternion.identity);
            projectile.Shooter = body;

            var force = new Vector2(horizontalForce * body.Value.transform.localScale.x , 0);
            projectile.SetForce(force);

            if(shakeCamera.Value)
            {
                cameraShake.Value.ShakeCamera(5f, 0.3f);
            }
        
        return Status.Success;
    }
}

