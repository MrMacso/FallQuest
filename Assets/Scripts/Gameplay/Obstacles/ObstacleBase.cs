using IntoTheVoid.Gameplay.Player;
using UnityEngine;

namespace IntoTheVoid.Gameplay.Obstacles
{
    public abstract class ObstacleBase : MonoBehaviour
    {
        [SerializeField] private bool disableAfterContact;

        public bool IsActive { get; private set; } = true;

        protected virtual void OnEnable()
        {
            IsActive = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsActive)
            {
                return;
            }

            PlayerMotor player = other.GetComponentInParent<PlayerMotor>();
            if (player == null)
            {
                return;
            }

            HandlePlayerContact(player);

            if (disableAfterContact)
            {
                IsActive = false;
            }
        }

        public virtual void ResetObstacle()
        {
            IsActive = true;
        }

        protected void Deactivate()
        {
            IsActive = false;
        }

        protected abstract void HandlePlayerContact(PlayerMotor player);
    }
}
