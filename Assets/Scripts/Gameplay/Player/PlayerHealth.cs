using System;
using UnityEngine;
using UnityEngine.Events;

namespace IntoTheVoid.Gameplay.Player
{
    public sealed class PlayerHealth : MonoBehaviour
    {
        [Serializable]
        public sealed class HealthChangedEvent : UnityEvent<float, float>
        {
        }

        [Header("Health")]
        [SerializeField, Min(1f)] private float maximumHealth = 100f;
        [SerializeField, Min(0f)] private float invulnerabilityDuration = 0.5f;

        [Header("Inspector events")]
        [Tooltip("Receives current health and maximum health.")]
        [SerializeField] private HealthChangedEvent onHealthChanged = new();
        [SerializeField] private UnityEvent onDamaged = new();
        [SerializeField] private UnityEvent onHealed = new();
        [SerializeField] private UnityEvent onDied = new();

        private float invulnerableUntil;

        public event Action<float, float> HealthChanged;
        public event Action<float> Damaged;
        public event Action<float> Healed;
        public event Action Died;

        public float CurrentHealth { get; private set; }
        public float MaximumHealth => maximumHealth;
        public float NormalizedHealth => maximumHealth > 0f ? CurrentHealth / maximumHealth : 0f;
        public bool IsAlive => CurrentHealth > 0f;
        public bool IsInvulnerable => Time.time < invulnerableUntil;

        private void Awake()
        {
            ResetHealth();
        }

        public bool TakeDamage(float amount)
        {
            if (!IsAlive || IsInvulnerable || amount <= 0f)
            {
                return false;
            }

            float previousHealth = CurrentHealth;
            CurrentHealth = Mathf.Max(0f, CurrentHealth - amount);
            float appliedDamage = previousHealth - CurrentHealth;

            if (CurrentHealth > 0f && invulnerabilityDuration > 0f)
            {
                invulnerableUntil = Time.time + invulnerabilityDuration;
            }

            Damaged?.Invoke(appliedDamage);
            onDamaged.Invoke();
            NotifyHealthChanged();

            if (CurrentHealth <= 0f)
            {
                Died?.Invoke();
                onDied.Invoke();
            }

            return true;
        }

        public bool Heal(float amount)
        {
            if (!IsAlive || amount <= 0f || CurrentHealth >= maximumHealth)
            {
                return false;
            }

            float previousHealth = CurrentHealth;
            CurrentHealth = Mathf.Min(maximumHealth, CurrentHealth + amount);
            float appliedHealing = CurrentHealth - previousHealth;

            Healed?.Invoke(appliedHealing);
            onHealed.Invoke();
            NotifyHealthChanged();
            return true;
        }

        public void Kill()
        {
            if (!IsAlive)
            {
                return;
            }

            float remainingHealth = CurrentHealth;
            CurrentHealth = 0f;
            invulnerableUntil = 0f;

            Damaged?.Invoke(remainingHealth);
            onDamaged.Invoke();
            NotifyHealthChanged();
            Died?.Invoke();
            onDied.Invoke();
        }

        public void ResetHealth()
        {
            maximumHealth = Mathf.Max(1f, maximumHealth);
            CurrentHealth = maximumHealth;
            invulnerableUntil = 0f;
            NotifyHealthChanged();
        }

        private void NotifyHealthChanged()
        {
            HealthChanged?.Invoke(CurrentHealth, maximumHealth);
            onHealthChanged.Invoke(CurrentHealth, maximumHealth);
        }

        private void OnValidate()
        {
            maximumHealth = Mathf.Max(1f, maximumHealth);
            invulnerabilityDuration = Mathf.Max(0f, invulnerabilityDuration);
        }
    }
}
