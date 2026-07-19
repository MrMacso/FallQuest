using IntoTheVoid.Gameplay.Speed;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IntoTheVoid.UI
{
    public sealed class FallSpeedDebugHUD : MonoBehaviour
    {
        [SerializeField] private FallSpeedController fallSpeed;
        [SerializeField] private TMP_Text speedText;
        [SerializeField] private TMP_Text zoneText;
        [SerializeField] private Slider speedSlider;
        [SerializeField] private Image zoneIndicator;

        [Header("Zone colors")]
        [SerializeField] private Color slowColor = new(0.25f, 0.75f, 1f);
        [SerializeField] private Color normalColor = Color.white;
        [SerializeField] private Color fastColor = new(1f, 0.65f, 0.1f);
        [SerializeField] private Color criticalColor = new(1f, 0.15f, 0.1f);

        private void OnEnable()
        {
            if (fallSpeed == null)
            {
                return;
            }

            fallSpeed.SpeedChanged += HandleSpeedChanged;
            fallSpeed.SpeedZoneChanged += HandleZoneChanged;
            RefreshAll();
        }

        private void OnDisable()
        {
            if (fallSpeed == null)
            {
                return;
            }

            fallSpeed.SpeedChanged -= HandleSpeedChanged;
            fallSpeed.SpeedZoneChanged -= HandleZoneChanged;
        }

        private void RefreshAll()
        {
            HandleSpeedChanged(fallSpeed.CurrentSpeed);
            HandleZoneChanged(fallSpeed.CurrentZone);
        }

        private void HandleSpeedChanged(float speed)
        {
            if (speedText != null)
            {
                speedText.text = $"{speed:0.0} m/s";
            }

            if (speedSlider != null)
            {
                speedSlider.minValue = fallSpeed.Config.MinimumSpeed;
                speedSlider.maxValue = fallSpeed.Config.MaximumSpeed;
                speedSlider.value = speed;
            }
        }

        private void HandleZoneChanged(SpeedZone zone)
        {
            if (zoneText != null)
            {
                zoneText.text = zone.ToString();
            }

            if (zoneIndicator != null)
            {
                zoneIndicator.color = GetZoneColor(zone);
            }
        }

        private Color GetZoneColor(SpeedZone zone)
        {
            return zone switch
            {
                SpeedZone.Slow => slowColor,
                SpeedZone.Normal => normalColor,
                SpeedZone.Fast => fastColor,
                SpeedZone.Critical => criticalColor,
                _ => normalColor
            };
        }
    }
}
