using IntoTheVoid.Gameplay.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IntoTheVoid.UI
{
    public sealed class PlayerHealthHUD : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Image healthFill;
        [SerializeField] private TMP_Text healthText;

        [Header("Health colors")]
        [SerializeField] private Color fullHealthColor = new(0.2f, 0.9f, 0.3f);
        [SerializeField] private Color lowHealthColor = new(0.95f, 0.15f, 0.1f);

        private void OnEnable()
        {
            if (playerHealth == null)
            {
                return;
            }

            playerHealth.HealthChanged += HandleHealthChanged;
            HandleHealthChanged(playerHealth.CurrentHealth, playerHealth.MaximumHealth);
        }

        private void OnDisable()
        {
            if (playerHealth != null)
            {
                playerHealth.HealthChanged -= HandleHealthChanged;
            }
        }

        private void HandleHealthChanged(float currentHealth, float maximumHealth)
        {
            float normalizedHealth = maximumHealth > 0f
                ? Mathf.Clamp01(currentHealth / maximumHealth)
                : 0f;

            if (healthSlider != null)
            {
                healthSlider.minValue = 0f;
                healthSlider.maxValue = maximumHealth;
                healthSlider.value = currentHealth;
            }

            if (healthFill != null)
            {
                healthFill.color = Color.Lerp(lowHealthColor, fullHealthColor, normalizedHealth);
            }

            if (healthText != null)
            {
                healthText.text = $"{Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maximumHealth)}";
            }
        }
    }
}
