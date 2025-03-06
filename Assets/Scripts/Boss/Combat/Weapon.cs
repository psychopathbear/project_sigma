using System;
using Scripts.Boss.Combat.Projectiles;
using UnityEngine;

namespace Scripts.Boss.Combat
{
    [Serializable]
    public class Weapon : MonoBehaviour
    {
        public Transform weaponTransform;
        public AbstractProjectile projectilePrefab;
        public float horizontalForce = 5.0f;
        public float verticalForce = 4.0f;
    }
}