using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

    public class Hittable : MonoBehaviour
    {
        public enum HitType
        {
            None,
            Inflate,
            Push,
            Color
        }

        private static readonly Color DeadColor = new Color(0.4f, 0.4f, 0.4f);
        private static readonly Color HitColor = new Color(0.2f, 0.0f, 0.0f);

        public HitType hitType = HitType.None;
        public bool disableHitEffect = false;
        public Transform spriteParent;
        public bool hideWhenDead = false;
        public Material hitMaterial;
        public GameObject customHitEffect; // Added missing field
        public AudioClip customHitSound; // Added missing field

        private SpriteRenderer sprite;
        private float baseScale;
        protected Color defaultColor = Color.white;
        private Material defaultMaterial;

        public event Action<Vector2, Vector2> OnHit;

        // Start is called before the first frame update
        protected virtual void Awake()
        {
            // Find all child sprite renderers
            Transform spriteParentTransform = spriteParent != null ? spriteParent : transform;
            sprite = spriteParentTransform.GetComponentInChildren<SpriteRenderer>();

            baseScale = transform.localScale.y;
            defaultMaterial = sprite.material;
        }

        public virtual void OnAttackHit(Vector2 position, Vector2 force, int damage)
        {
            // Hurt/Damage
            OnHit?.Invoke(position, force);

            if (hitType == HitType.Inflate)
            {
                // Replace Tween.LocalScale with DOTween
                transform.DOScale(
                    new Vector3(transform.localScale.x * 1.01f, baseScale + 0.05f, baseScale), 
                    0.5f)
                    .From(new Vector3(transform.localScale.x, baseScale, baseScale))
                    .SetEase(Ease.InOutElastic);
            }
            else if (hitType == HitType.Push)
            {
                // Push object quickly by a small amount and return to its original position
                float hitAmount = 0.05f;
                Vector3 targetPos = transform.position + new Vector3(
                    UnityEngine.Random.Range(-hitAmount, hitAmount),
                    UnityEngine.Random.Range(-hitAmount, hitAmount), 
                    0);
                    
                // Replace Tween.Position with DOTween
                transform.DOMove(targetPos, 0.5f)
                    .SetEase(Ease.InOutElastic)
                    .OnComplete(() => transform.DOMove(transform.position, 0.5f));
            }
            else if (hitType == HitType.Color)
            {
                // Impact color flash
                sprite.material = hitMaterial;
                StartCoroutine(ResetMaterial(0.1f));
            }

            // Impact particle effect
            /*if (!disableHitEffect)
            {
                if (customHitEffect != null)
                    EffectManager.Instance.PlayOneShot(customHitEffect, position);

                CameraController.Instance.ShakeCamera(0.03f, 0.5f);
            }

            if (customHitSound != null)
                SoundManager.Instance.PlaySoundAtLocation(customHitSound, transform.position);*/
        }

        private IEnumerator ResetMaterial(float delay)
        {
            yield return new WaitForSeconds(delay);
            sprite.material = defaultMaterial;
        }
    }