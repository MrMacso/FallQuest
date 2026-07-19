using System;

namespace IntoTheVoid.Gameplay.Speed
{
    public enum SpeedZone
    {
        Slow,
        Normal,
        Fast,
        Critical
    }

    public static class SpeedZoneEvaluator
    {
        public static SpeedZone Evaluate(float speed, FallSpeedConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (speed <= config.SlowUpperLimit)
            {
                return SpeedZone.Slow;
            }

            if (speed <= config.NormalUpperLimit)
            {
                return SpeedZone.Normal;
            }

            if (speed <= config.FastUpperLimit)
            {
                return SpeedZone.Fast;
            }

            return SpeedZone.Critical;
        }
    }
}
