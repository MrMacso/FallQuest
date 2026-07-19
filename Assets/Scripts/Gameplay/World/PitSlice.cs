using IntoTheVoid.Gameplay.Obstacles;
using IntoTheVoid.Gameplay.Pickups;
using UnityEngine;

namespace IntoTheVoid.Gameplay.World
{
    public sealed class PitSlice : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float length = 20f;

        public float Length => length;

        public void PrepareForReuse()
        {
            foreach (ObstacleBase obstacle in GetComponentsInChildren<ObstacleBase>(true))
            {
                obstacle.ResetObstacle();
            }

            foreach (PickupBase pickup in GetComponentsInChildren<PickupBase>(true))
            {
                pickup.ResetPickup();
            }
        }

        private void OnValidate()
        {
            length = Mathf.Max(0.1f, length);
        }
    }
}
