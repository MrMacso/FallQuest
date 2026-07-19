using System;
using IntoTheVoid.Input;
using UnityEngine;

namespace IntoTheVoid.Gameplay.Speed
{
    public sealed class FallSpeedController : MonoBehaviour
    {
        [SerializeField] private FallSpeedConfig config;
        [SerializeField] private PlayerInputReader input;

        private SpeedZone currentZone;

        public event Action<float> SpeedChanged;
        public event Action<SpeedZone> SpeedZoneChanged;

        public FallSpeedConfig Config => config;
        public float CurrentSpeed { get; private set; }
        public SpeedZone CurrentZone => currentZone;
        public float NormalizedSpeed => config != null ? config.NormalizeSpeed(CurrentSpeed) : 0f;

        private void Awake()
        {
            if (config == null)
            {
                Debug.LogError($"{nameof(FallSpeedController)} on '{name}' requires a speed config.", this);
                enabled = false;
                return;
            }

            ResetSpeed();
        }

        private void Update()
        {
            float accelerationInput = input != null ? input.Accelerate : 0f;
            float brakeInput = input != null ? input.Brake : 0f;
            Tick(Time.deltaTime, accelerationInput, brakeInput);
        }

        public void Tick(float deltaTime, float accelerationInput, float brakeInput)
        {
            if (config == null || deltaTime <= 0f)
            {
                return;
            }

            accelerationInput = Mathf.Clamp01(accelerationInput);
            brakeInput = Mathf.Clamp01(brakeInput);

            float acceleration = config.GravityAcceleration
                                 + config.PlayerAcceleration * accelerationInput
                                 - config.PlayerBraking * brakeInput;

            SetSpeed(CurrentSpeed + acceleration * deltaTime);
        }

        public void ResetSpeed()
        {
            if (config == null)
            {
                return;
            }

            CurrentSpeed = Mathf.Clamp(config.StartingSpeed, config.MinimumSpeed, config.MaximumSpeed);
            currentZone = SpeedZoneEvaluator.Evaluate(CurrentSpeed, config);
            SpeedChanged?.Invoke(CurrentSpeed);
            SpeedZoneChanged?.Invoke(currentZone);
        }

        public void SetSpeed(float speed)
        {
            if (config == null)
            {
                return;
            }

            float clampedSpeed = Mathf.Clamp(speed, config.MinimumSpeed, config.MaximumSpeed);
            SpeedZone newZone = SpeedZoneEvaluator.Evaluate(clampedSpeed, config);
            bool speedHasChanged = !Mathf.Approximately(CurrentSpeed, clampedSpeed);
            bool zoneHasChanged = currentZone != newZone;

            CurrentSpeed = clampedSpeed;
            currentZone = newZone;

            if (speedHasChanged)
            {
                SpeedChanged?.Invoke(CurrentSpeed);
            }

            if (zoneHasChanged)
            {
                SpeedZoneChanged?.Invoke(currentZone);
            }
        }
    }
}
