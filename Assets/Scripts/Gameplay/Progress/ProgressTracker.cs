using System;
using IntoTheVoid.Gameplay.World;
using UnityEngine;

namespace IntoTheVoid.Gameplay.Progress
{
    public sealed class ProgressTracker : MonoBehaviour
    {
        [SerializeField] private PitStreamController pitStream;
        [SerializeField, Min(1f)] private float levelDepth = 1000f;

        public event Action<float, float> DepthChanged;
        public event Action<float> NormalizedProgressChanged;
        public event Action Completed;

        public float CurrentDepth { get; private set; }
        public float LevelDepth => levelDepth;
        public float NormalizedProgress => Mathf.Clamp01(CurrentDepth / levelDepth);
        public bool IsCompleted { get; private set; }

        private void OnEnable()
        {
            if (pitStream != null)
            {
                pitStream.Scrolled += AddDepth;
            }
        }

        private void OnDisable()
        {
            if (pitStream != null)
            {
                pitStream.Scrolled -= AddDepth;
            }
        }

        public void AddDepth(float distance)
        {
            if (IsCompleted || distance <= 0f)
            {
                return;
            }

            CurrentDepth = Mathf.Min(levelDepth, CurrentDepth + distance);
            NotifyProgressChanged();

            if (CurrentDepth >= levelDepth)
            {
                IsCompleted = true;
                Completed?.Invoke();
            }
        }

        public void ResetProgress()
        {
            CurrentDepth = 0f;
            IsCompleted = false;
            NotifyProgressChanged();
        }

        private void NotifyProgressChanged()
        {
            DepthChanged?.Invoke(CurrentDepth, levelDepth);
            NormalizedProgressChanged?.Invoke(NormalizedProgress);
        }

        private void OnValidate()
        {
            levelDepth = Mathf.Max(1f, levelDepth);
        }
    }
}
