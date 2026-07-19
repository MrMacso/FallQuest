using UnityEngine;

namespace IntoTheVoid.Gameplay.Speed
{
    [CreateAssetMenu(
        fileName = "FallSpeedConfig",
        menuName = "Into The Void/Gameplay/Fall Speed Config")]
    public sealed class FallSpeedConfig : ScriptableObject
    {
        [Header("Speed")]
        [SerializeField, Min(0f)] private float minimumSpeed = 15f;
        [SerializeField, Min(0f)] private float maximumSpeed = 60f;
        [SerializeField, Min(0f)] private float startingSpeed = 20f;

        [Header("Acceleration")]
        [Tooltip("Passive acceleration applied every second.")]
        [SerializeField, Min(0f)] private float gravityAcceleration = 1.5f;
        [SerializeField, Min(0f)] private float playerAcceleration = 12f;
        [SerializeField, Min(0f)] private float playerBraking = 18f;

        [Header("Speed zone upper limits")]
        [SerializeField, Min(0f)] private float slowUpperLimit = 25f;
        [SerializeField, Min(0f)] private float normalUpperLimit = 40f;
        [SerializeField, Min(0f)] private float fastUpperLimit = 52f;

        public float MinimumSpeed => minimumSpeed;
        public float MaximumSpeed => maximumSpeed;
        public float StartingSpeed => startingSpeed;
        public float GravityAcceleration => gravityAcceleration;
        public float PlayerAcceleration => playerAcceleration;
        public float PlayerBraking => playerBraking;
        public float SlowUpperLimit => slowUpperLimit;
        public float NormalUpperLimit => normalUpperLimit;
        public float FastUpperLimit => fastUpperLimit;

        public float NormalizeSpeed(float speed)
        {
            return Mathf.InverseLerp(minimumSpeed, maximumSpeed, speed);
        }

        private void OnValidate()
        {
            maximumSpeed = Mathf.Max(minimumSpeed, maximumSpeed);
            startingSpeed = Mathf.Clamp(startingSpeed, minimumSpeed, maximumSpeed);

            slowUpperLimit = Mathf.Clamp(slowUpperLimit, minimumSpeed, maximumSpeed);
            normalUpperLimit = Mathf.Clamp(normalUpperLimit, slowUpperLimit, maximumSpeed);
            fastUpperLimit = Mathf.Clamp(fastUpperLimit, normalUpperLimit, maximumSpeed);
        }
    }
}
