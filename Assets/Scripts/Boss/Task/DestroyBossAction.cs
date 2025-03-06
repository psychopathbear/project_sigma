using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Scripts.Util;
using DG.Tweening;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "DestroyBoss", story: "Destroy Boss", category: "Action", id: "c89e1880fd92ae4e9459e492dad46d8e")]
public partial class DestroyBossAction : EnemyAction
{
    public float bleedTime = 1.0f;
    [SerializeReference] public BlackboardVariable<ParticleSystem> bleedEffect;
    [SerializeReference] public BlackboardVariable<ParticleSystem> explodeEffect;

    private bool isDestroyed;
    protected override Status OnStart()
    {
        EffectManager.Instance.PlayOneShot(bleedEffect.Value, body.Value.transform.position);
        DOVirtual.DelayedCall(bleedTime, () =>
        {
            EffectManager.Instance.PlayOneShot(explodeEffect.Value, body.Value.transform.position);
            cameraShake.Value.ShakeCamera(10f, 0.8f);
            body.Value.SetActive(false);
        },false);
        isDestroyed = true;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return isDestroyed ? Status.Success : Status.Running;
    }

}

