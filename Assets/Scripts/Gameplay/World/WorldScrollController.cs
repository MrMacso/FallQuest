using System;
using IntoTheVoid.Gameplay.Speed;
using UnityEngine;

namespace IntoTheVoid.Gameplay.World
{
    public sealed class WorldScrollController : MonoBehaviour
    {
        [SerializeField] private FallSpeedController fallSpeed;
        [Tooltip("Parent of the test environment. This object is used when left empty.")]
        [SerializeField] private Transform scrollRoot;
        [SerializeField] private Vector3 scrollDirection = Vector3.up;
        [SerializeField, Min(0f)] private float speedMultiplier = 1f;

        public event Action<float> Scrolled;

        public float ScrolledDistance { get; private set; }

        private void Awake()
        {
            if (scrollRoot == null)
            {
                scrollRoot = transform;
            }
        }

        private void Update()
        {
            if (fallSpeed == null)
            {
                return;
            }

            Scroll(fallSpeed.CurrentSpeed, Time.deltaTime);
        }

        public void Scroll(float fallSpeedValue, float deltaTime)
        {
            if (scrollRoot == null || deltaTime <= 0f)
            {
                return;
            }

            float distance = Mathf.Max(0f, fallSpeedValue) * speedMultiplier * deltaTime;
            Vector3 direction = scrollDirection.sqrMagnitude > 0f
                ? scrollDirection.normalized
                : Vector3.up;

            scrollRoot.Translate(direction * distance, Space.World);
            ScrolledDistance += distance;
            Scrolled?.Invoke(distance);
        }

        public void ResetDistance()
        {
            ScrolledDistance = 0f;
        }
    }
}
