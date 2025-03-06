using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using DG.Tweening;

namespace Scripts.AI
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Jump", story: "Jump", category: "Action", id: "ff453ea4df265beef73b247dbf04d5d6")]
    public partial class JumpAction : EnemyAction
    {
        [SerializeReference] public BlackboardVariable<float> horizontalForce;
        [SerializeReference] public BlackboardVariable<float> jumpForce;

        [SerializeReference] public BlackboardVariable<float> buildupTime;
        [SerializeReference] public BlackboardVariable<float> jumpTime;

        [SerializeReference] public BlackboardVariable<string> animationTriggerName;
        [SerializeReference] public BlackboardVariable<bool> shakeCameraOnLanding;
        [SerializeReference] public BlackboardVariable<GameObject> hazardCollider;

        private bool hasLanded;

        private Tween buildupTween;
        private Tween jumpTween;

        protected override Status OnStart()
        {
            if (enemyAnimator == null)
            {
                Debug.LogError("animator is not set.");
                return Status.Failure;
            }

            buildupTween = DOVirtual.DelayedCall(buildupTime, StartJump, false);
            enemyAnimator.Value.SetTrigger(animationTriggerName);
            return Status.Running;
        }

        private void StartJump()
        {
            var direction = player.Value.transform.position.x < body.Value.transform.position.x ? -1 : 1; // Jump towards player
            var bodyRigidbody = body.Value.GetComponent<Rigidbody2D>();
            if (bodyRigidbody == null)
            {
                Debug.LogError("Rigidbody2D component is not found on body.");
                return;
            }
            bodyRigidbody.AddForce(new Vector2(horizontalForce.Value * direction, jumpForce.Value), ForceMode2D.Impulse);
            jumpTween = DOVirtual.DelayedCall(jumpTime.Value, () =>
            {
                hasLanded = true;
                if (shakeCameraOnLanding.Value)
                {
                    cameraShake.Value.ShakeCamera(10f, 0.8f);
                }
            }, false);
            //hazardCollider.Value.SetActive(true);
        } 

        protected override Status OnUpdate()
        {
            // check lineer velocity if little than zero, then return success
            if(body.Value.GetComponent<Rigidbody2D>().linearVelocityY < 0)
            {
                hazardCollider.Value.SetActive(true);
            }
            else if(body.Value.GetComponent<Rigidbody2D>().linearVelocityY == 0)
            {
                hazardCollider.Value.SetActive(false);
            }
            else
            {
                hazardCollider.Value.SetActive(false);
            }
            return hasLanded ? Status.Success : Status.Running;
        }

        protected override void OnEnd()
        {
            buildupTween?.Kill();
            jumpTween?.Kill();
            hasLanded = false;
        }
    }
}
