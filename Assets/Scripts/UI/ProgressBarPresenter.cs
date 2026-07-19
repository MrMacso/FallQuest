using IntoTheVoid.Gameplay.Progress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IntoTheVoid.UI
{
    public sealed class ProgressBarPresenter : MonoBehaviour
    {
        [SerializeField] private ProgressTracker progressTracker;
        [SerializeField] private Slider progressSlider;
        [SerializeField] private TMP_Text depthText;

        private void OnEnable()
        {
            if (progressTracker == null)
            {
                return;
            }

            progressTracker.NormalizedProgressChanged += HandleNormalizedProgressChanged;
            progressTracker.DepthChanged += HandleDepthChanged;
            Refresh();
        }

        private void OnDisable()
        {
            if (progressTracker == null)
            {
                return;
            }

            progressTracker.NormalizedProgressChanged -= HandleNormalizedProgressChanged;
            progressTracker.DepthChanged -= HandleDepthChanged;
        }

        private void Refresh()
        {
            HandleNormalizedProgressChanged(progressTracker.NormalizedProgress);
            HandleDepthChanged(progressTracker.CurrentDepth, progressTracker.LevelDepth);
        }

        private void HandleNormalizedProgressChanged(float progress)
        {
            if (progressSlider == null)
            {
                return;
            }

            progressSlider.minValue = 0f;
            progressSlider.maxValue = 1f;
            progressSlider.value = progress;
        }

        private void HandleDepthChanged(float currentDepth, float levelDepth)
        {
            if (depthText != null)
            {
                depthText.text = $"{currentDepth:0} / {levelDepth:0} m";
            }
        }
    }
}
