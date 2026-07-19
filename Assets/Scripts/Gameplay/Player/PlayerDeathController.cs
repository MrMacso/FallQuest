using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace IntoTheVoid.Gameplay.Player
{
    public sealed class PlayerDeathController : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;

        [Header("Objects controlled by death")]
        [Tooltip("Typically PlayerMotor, PlayerInputReader, FallSpeedController and WorldScrollController.")]
        [SerializeField] private Behaviour[] disableOnDeath;
        [SerializeField] private GameObject showOnDeath;

        [Header("Restart")]
        [SerializeField] private bool restartAutomatically;
        [SerializeField, Min(0f)] private float automaticRestartDelay = 2f;
        [SerializeField] private bool useUnscaledTime = true;

        [Header("Inspector events")]
        [SerializeField] private UnityEvent onPlayerDied = new();
        [SerializeField] private UnityEvent onRestarting = new();

        private Coroutine restartRoutine;

        public bool IsDead { get; private set; }

        private void Awake()
        {
            if (showOnDeath != null)
            {
                showOnDeath.SetActive(false);
            }
        }

        private void OnEnable()
        {
            if (playerHealth != null)
            {
                playerHealth.Died += HandlePlayerDied;
            }
        }

        private void OnDisable()
        {
            if (playerHealth != null)
            {
                playerHealth.Died -= HandlePlayerDied;
            }

            if (restartRoutine != null)
            {
                StopCoroutine(restartRoutine);
                restartRoutine = null;
            }
        }

        private void HandlePlayerDied()
        {
            if (IsDead)
            {
                return;
            }

            IsDead = true;
            SetControlledBehavioursEnabled(false);

            if (showOnDeath != null)
            {
                showOnDeath.SetActive(true);
            }

            onPlayerDied.Invoke();

            if (restartAutomatically)
            {
                restartRoutine = StartCoroutine(RestartAfterDelay());
            }
        }

        public void RestartScene()
        {
            onRestarting.Invoke();
            Scene activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(activeScene.buildIndex);
        }

        public void ReviveWithoutReloading()
        {
            if (!IsDead || playerHealth == null)
            {
                return;
            }

            if (restartRoutine != null)
            {
                StopCoroutine(restartRoutine);
                restartRoutine = null;
            }

            playerHealth.ResetHealth();
            IsDead = false;
            SetControlledBehavioursEnabled(true);

            if (showOnDeath != null)
            {
                showOnDeath.SetActive(false);
            }
        }

        private IEnumerator RestartAfterDelay()
        {
            if (useUnscaledTime)
            {
                yield return new WaitForSecondsRealtime(automaticRestartDelay);
            }
            else
            {
                yield return new WaitForSeconds(automaticRestartDelay);
            }

            restartRoutine = null;
            RestartScene();
        }

        private void SetControlledBehavioursEnabled(bool value)
        {
            if (disableOnDeath == null)
            {
                return;
            }

            foreach (Behaviour controlledBehaviour in disableOnDeath)
            {
                if (controlledBehaviour != null)
                {
                    controlledBehaviour.enabled = value;
                }
            }
        }
    }
}
