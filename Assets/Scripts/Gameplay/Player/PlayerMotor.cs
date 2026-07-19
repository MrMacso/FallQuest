using IntoTheVoid.Gameplay.Speed;
using IntoTheVoid.Input;
using UnityEngine;

namespace IntoTheVoid.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class PlayerMotor : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerInputReader input;
        [SerializeField] private FallSpeedController fallSpeed;
        [SerializeField] private Transform shaftCenter;

        [Header("Movement")]
        [SerializeField, Min(0f)] private float lateralSpeed = 8f;
        [Tooltip("Steering multiplier at minimum and maximum fall speed.")]
        [SerializeField] private Vector2 speedControlRange = new(1.15f, 0.65f);
        [SerializeField, Min(0f)] private float shaftRadius = 5f;

        private CharacterController characterController;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (input == null)
            {
                return;
            }

            Move(input.Move, Time.deltaTime);
        }

        public void Move(Vector2 moveInput, float deltaTime)
        {
            if (characterController == null || deltaTime <= 0f)
            {
                return;
            }

            Vector2 clampedInput = Vector2.ClampMagnitude(moveInput, 1f);
            float normalizedFallSpeed = fallSpeed != null ? fallSpeed.NormalizedSpeed : 0f;
            float controlMultiplier = Mathf.Lerp(
                speedControlRange.x,
                speedControlRange.y,
                normalizedFallSpeed);

            Vector3 displacement = new Vector3(clampedInput.x, 0f, clampedInput.y)
                                   * (lateralSpeed * controlMultiplier * deltaTime);

            characterController.Move(displacement);
            ClampToShaft();
        }

        private void ClampToShaft()
        {
            Vector3 center = shaftCenter != null ? shaftCenter.position : Vector3.zero;
            Vector3 offset = transform.position - center;
            Vector2 planarOffset = new(offset.x, offset.z);

            if (planarOffset.sqrMagnitude <= shaftRadius * shaftRadius)
            {
                return;
            }

            Vector2 clampedOffset = planarOffset.normalized * shaftRadius;
            Vector3 targetPosition = new(
                center.x + clampedOffset.x,
                transform.position.y,
                center.z + clampedOffset.y);

            characterController.Move(targetPosition - transform.position);
        }

        private void OnValidate()
        {
            speedControlRange.x = Mathf.Max(0f, speedControlRange.x);
            speedControlRange.y = Mathf.Max(0f, speedControlRange.y);
        }
    }
}
