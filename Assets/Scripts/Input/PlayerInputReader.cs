using UnityEngine;
using UnityEngine.InputSystem;

namespace IntoTheVoid.Input
{
    public sealed class PlayerInputReader : MonoBehaviour
    {
        [Header("Input actions")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference accelerateAction;
        [SerializeField] private InputActionReference brakeAction;

        private bool enabledMoveAction;
        private bool enabledAccelerateAction;
        private bool enabledBrakeAction;

        public Vector2 Move => ReadVector2(moveAction);
        public float Accelerate => ReadButtonValue(accelerateAction);
        public float Brake => ReadButtonValue(brakeAction);

        private void OnEnable()
        {
            enabledMoveAction = EnableIfNeeded(moveAction);
            enabledAccelerateAction = EnableIfNeeded(accelerateAction);
            enabledBrakeAction = EnableIfNeeded(brakeAction);
        }

        private void OnDisable()
        {
            DisableIfOwned(moveAction, enabledMoveAction);
            DisableIfOwned(accelerateAction, enabledAccelerateAction);
            DisableIfOwned(brakeAction, enabledBrakeAction);
        }

        private static Vector2 ReadVector2(InputActionReference actionReference)
        {
            return actionReference != null ? actionReference.action.ReadValue<Vector2>() : Vector2.zero;
        }

        private static float ReadButtonValue(InputActionReference actionReference)
        {
            return actionReference != null ? actionReference.action.ReadValue<float>() : 0f;
        }

        private static bool EnableIfNeeded(InputActionReference actionReference)
        {
            if (actionReference == null || actionReference.action.enabled)
            {
                return false;
            }

            actionReference.action.Enable();
            return true;
        }

        private static void DisableIfOwned(InputActionReference actionReference, bool owned)
        {
            if (owned && actionReference != null)
            {
                actionReference.action.Disable();
            }
        }
    }
}
