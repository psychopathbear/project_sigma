using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "EnemyAction", story: "Enemy Action", category: "Action", id: "64a4e8612055f075bcb1380e1d588b02")]
public partial class EnemyAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> body;
    [SerializeReference] public BlackboardVariable<Animator> enemyAnimator;
    [SerializeReference] public BlackboardVariable<EnemyHealth> enemyHealth;
    [SerializeReference] public BlackboardVariable<Collider2D> enemyCollider;
    [SerializeReference] public BlackboardVariable<PlayerMovement> player;
    [SerializeReference] public BlackboardVariable<CameraController> cameraShake;
    [SerializeReference] public BlackboardVariable<int> currentStage;
    [SerializeReference] public BlackboardVariable<int> HealthThreshold;

    protected override Status OnStart()
    {       
        if (body == null)
            body = new BlackboardVariable<GameObject>();
            
        if (player == null)
            player = new BlackboardVariable<PlayerMovement>();
            
        if (enemyAnimator == null)
            enemyAnimator = new BlackboardVariable<Animator>();
            
        enemyHealth.Value = GameObject.GetComponent<EnemyHealth>();
        enemyCollider.Value = GameObject.GetComponent<Collider2D>();
        
        return base.OnStart();
    }
}

