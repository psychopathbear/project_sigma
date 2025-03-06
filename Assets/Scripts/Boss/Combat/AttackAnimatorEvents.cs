using Scripts.Util;
using UnityEngine;

namespace Scripts.Boss.Combat
{
    public class AttackAnimatorEvents : MonoBehaviour
    {
        public GameObject attackCollider;
        public ParticleSystem impactEffect;
        public Transform impactTransform;
        public float cameraShakeIntensity = 0.2f;
        
        private void OnAttackStart()
        {
            if (attackCollider != null)
            {
                attackCollider.SetActive(true);
            }
            else
            {
                Debug.LogError("attackCollider is not assigned.");
            }

            if (impactEffect != null && impactTransform != null)
            {
                EffectManager.Instance?.PlayOneShot(impactEffect, impactTransform.position);
            }
            else
            {
                Debug.LogError("impactEffect or impactTransform is not assigned.");
            }

            //CameraController.Instance?.ShakeCamera(cameraShakeIntensity);
        }
        
        private void OnAttackEnd()
        {
            if (attackCollider != null)
            {
                attackCollider.SetActive(false);
            }
            else
            {
                Debug.LogError("attackCollider is not assigned.");
            }
        }
    }
}