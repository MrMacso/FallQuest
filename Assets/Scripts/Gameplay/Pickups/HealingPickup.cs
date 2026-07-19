using IntoTheVoid.Gameplay.Player;
using UnityEngine;

namespace IntoTheVoid.Gameplay.Pickups
{
    public sealed class HealingPickup : PickupBase
    {
        [SerializeField, Min(0f)] private float healingAmount = 25f;

        protected override bool ApplyEffect(PlayerMotor player)
        {
            PlayerHealth health = player.GetComponentInParent<PlayerHealth>();
            return health != null && health.Heal(healingAmount);
        }
    }
}
