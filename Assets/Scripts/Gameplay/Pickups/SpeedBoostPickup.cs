using IntoTheVoid.Gameplay.Player;
using IntoTheVoid.Gameplay.Speed;
using UnityEngine;

namespace IntoTheVoid.Gameplay.Pickups
{
    public sealed class SpeedBoostPickup : PickupBase
    {
        [SerializeField, Min(0f)] private float speedIncrease = 12f;

        protected override bool ApplyEffect(PlayerMotor player)
        {
            FallSpeedController speed = player.GetComponentInParent<FallSpeedController>();
            if (speed == null || speed.CurrentSpeed >= speed.Config.MaximumSpeed)
            {
                return false;
            }

            speed.SetSpeed(speed.CurrentSpeed + speedIncrease);
            return true;
        }
    }
}
