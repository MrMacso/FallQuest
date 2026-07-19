using IntoTheVoid.Gameplay.Player;
using UnityEngine;
using UnityEngine.Events;

namespace IntoTheVoid.Gameplay.Pickups
{
    public abstract class PickupBase : MonoBehaviour
    {
        [SerializeField] private Collider pickupCollider;
        [SerializeField] private GameObject visualRoot;
        [SerializeField] private UnityEvent onCollected = new();

        public bool IsAvailable { get; private set; } = true;

        protected virtual void OnEnable()
        {
            IsAvailable = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsAvailable)
            {
                return;
            }

            PlayerMotor player = other.GetComponentInParent<PlayerMotor>();
            if (player == null || !ApplyEffect(player))
            {
                return;
            }

            IsAvailable = false;
            SetPresentationActive(false);
            onCollected.Invoke();
        }

        public virtual void ResetPickup()
        {
            IsAvailable = true;
            SetPresentationActive(true);
        }

        protected abstract bool ApplyEffect(PlayerMotor player);

        private void SetPresentationActive(bool value)
        {
            if (pickupCollider != null)
            {
                pickupCollider.enabled = value;
            }

            if (visualRoot != null)
            {
                visualRoot.SetActive(value);
            }
        }
    }
}
