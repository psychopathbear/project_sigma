using System;
using Scripts.Util;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Boss.Combat.Projectiles
{
    public abstract class AbstractProjectile : MonoBehaviour
    {
        public int damage;
        public ParticleSystem explosionEffect;
        public AudioClip splatterSound;

        public GameObject Shooter { get; set; }

        protected Vector2 force;

        public event Action<AbstractProjectile> OnProjectileDestroyed;
    
        public abstract void SetForce(Vector2 force);

        protected void DestroyProjectile()
        {
            OnProjectileDestroyed?.Invoke(this);

            if (splatterSound != null)
            {
                //SoundManager.Instance.PlaySoundAtLocation(splatterSound, transform.position, 0.75f);
            }

            if (explosionEffect != null)
            {
                EffectManager.Instance.PlayOneShot(explosionEffect, transform.position);
            }
            else
            {
                Debug.LogWarning("Explosion effect is null");
            }

            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Can't shoot yourself
            if (collision.gameObject == Shooter)
                return;

            // Projectile hit player
            var player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                Vector2 force = this.force.normalized;
                player.TakeDamage(damage, transform.position); 
            }

            if(collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                DestroyProjectile();
            }
        }
    }
}