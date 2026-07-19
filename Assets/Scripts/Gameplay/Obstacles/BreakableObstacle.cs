using IntoTheVoid.Gameplay.Player;
using IntoTheVoid.Gameplay.Speed;
using UnityEngine;
using UnityEngine.Events;

namespace IntoTheVoid.Gameplay.Obstacles
{
    public sealed class BreakableObstacle : ObstacleBase
    {
        [SerializeField, Min(0f)] private float requiredSpeed = 45f;
        [SerializeField, Min(0f)] private float collisionDamage = 20f;
        [SerializeField] private Collider obstacleCollider;
        [SerializeField] private GameObject intactVisual;
        [SerializeField] private GameObject brokenVisual;
        [SerializeField] private UnityEvent onBroken = new();
        [SerializeField] private UnityEvent onFailedBreak = new();

        public float RequiredSpeed => requiredSpeed;
        public bool IsBroken { get; private set; }

        protected override void OnEnable()
        {
            base.OnEnable();
            RestoreVisuals();
        }

        protected override void HandlePlayerContact(PlayerMotor player)
        {
            FallSpeedController speed = player.GetComponentInParent<FallSpeedController>();
            if (speed != null && speed.CurrentSpeed >= requiredSpeed)
            {
                Break();
                return;
            }

            PlayerHealth health = player.GetComponentInParent<PlayerHealth>();
            health?.TakeDamage(collisionDamage);
            onFailedBreak.Invoke();
        }

        public void Break()
        {
            if (IsBroken)
            {
                return;
            }

            IsBroken = true;
            Deactivate();

            if (obstacleCollider != null)
            {
                obstacleCollider.enabled = false;
            }

            if (intactVisual != null)
            {
                intactVisual.SetActive(false);
            }

            if (brokenVisual != null)
            {
                brokenVisual.SetActive(true);
            }

            onBroken.Invoke();
        }

        public override void ResetObstacle()
        {
            base.ResetObstacle();
            RestoreVisuals();
        }

        private void RestoreVisuals()
        {
            IsBroken = false;

            if (obstacleCollider != null)
            {
                obstacleCollider.enabled = true;
            }

            if (intactVisual != null)
            {
                intactVisual.SetActive(true);
            }

            if (brokenVisual != null)
            {
                brokenVisual.SetActive(false);
            }
        }
    }
}
