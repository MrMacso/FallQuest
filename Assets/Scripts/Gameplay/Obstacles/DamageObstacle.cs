using System;
using IntoTheVoid.Gameplay.Player;
using UnityEngine;
using UnityEngine.Events;

namespace IntoTheVoid.Gameplay.Obstacles
{
    public sealed class DamageObstacle : ObstacleBase
    {
        [Serializable]
        public sealed class DamageEvent : UnityEvent<float>
        {
        }

        [SerializeField, Min(0f)] private float damage = 10f;
        [Tooltip("Temporary Inspector event until PlayerHealth is implemented.")]
        [SerializeField] private DamageEvent onDamageRequested = new();

        public event Action<float> DamageRequested;

        public float Damage => damage;

        protected override void HandlePlayerContact(PlayerMotor player)
        {
            PlayerHealth playerHealth = player.GetComponentInParent<PlayerHealth>();
            playerHealth?.TakeDamage(damage);

            DamageRequested?.Invoke(damage);
            onDamageRequested.Invoke(damage);
        }
    }
}
