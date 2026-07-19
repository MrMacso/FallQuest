using IntoTheVoid.Gameplay.Speed;
using UnityEngine;

namespace IntoTheVoid.Gameplay.Feedback
{
    public sealed class FallFeedbackController : MonoBehaviour
    {
        [SerializeField] private FallSpeedController fallSpeed;

        [Header("Camera")]
        [SerializeField] private Camera targetCamera;
        [SerializeField, Min(1f)] private float slowFieldOfView = 55f;
        [SerializeField, Min(1f)] private float fastFieldOfView = 75f;
        [SerializeField, Min(0f)] private float fieldOfViewResponse = 6f;

        [Header("Wind particles")]
        [SerializeField] private ParticleSystem windParticles;
        [SerializeField, Min(0f)] private float slowEmissionRate = 5f;
        [SerializeField, Min(0f)] private float fastEmissionRate = 100f;
        [SerializeField, Min(0f)] private float slowParticleSpeed = 3f;
        [SerializeField, Min(0f)] private float fastParticleSpeed = 18f;

        [Header("Wind audio")]
        [SerializeField] private AudioSource windAudio;
        [SerializeField, Range(0f, 1f)] private float slowVolume = 0.1f;
        [SerializeField, Range(0f, 1f)] private float fastVolume = 0.85f;
        [SerializeField, Range(-3f, 3f)] private float slowPitch = 0.8f;
        [SerializeField, Range(-3f, 3f)] private float fastPitch = 1.35f;

        private void Update()
        {
            if (fallSpeed == null)
            {
                return;
            }

            ApplyFeedback(fallSpeed.NormalizedSpeed, Time.deltaTime);
        }

        public void ApplyFeedback(float normalizedSpeed, float deltaTime)
        {
            normalizedSpeed = Mathf.Clamp01(normalizedSpeed);

            if (targetCamera != null)
            {
                float targetFov = Mathf.Lerp(slowFieldOfView, fastFieldOfView, normalizedSpeed);
                float blend = 1f - Mathf.Exp(-fieldOfViewResponse * Mathf.Max(0f, deltaTime));
                targetCamera.fieldOfView = Mathf.Lerp(targetCamera.fieldOfView, targetFov, blend);
            }

            if (windParticles != null)
            {
                ParticleSystem.EmissionModule emission = windParticles.emission;
                emission.rateOverTime = Mathf.Lerp(slowEmissionRate, fastEmissionRate, normalizedSpeed);

                ParticleSystem.MainModule main = windParticles.main;
                main.startSpeed = Mathf.Lerp(slowParticleSpeed, fastParticleSpeed, normalizedSpeed);
            }

            if (windAudio != null)
            {
                windAudio.volume = Mathf.Lerp(slowVolume, fastVolume, normalizedSpeed);
                windAudio.pitch = Mathf.Lerp(slowPitch, fastPitch, normalizedSpeed);
            }
        }
    }
}
