using System;
using IntoTheVoid.Gameplay.Player;
using IntoTheVoid.Gameplay.Progress;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace IntoTheVoid.Gameplay.Flow
{
    public enum GameFlowState
    {
        Preparing,
        Playing,
        Paused,
        Won,
        Lost
    }

    public sealed class GameFlowController : MonoBehaviour
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private ProgressTracker progressTracker;

        [Header("Gameplay behaviours")]
        [Tooltip("Player input, movement, fall speed, pit stream and feedback. Do not add this controller.")]
        [SerializeField] private Behaviour[] gameplayBehaviours;

        [Header("Panels")]
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject victoryPanel;
        [SerializeField] private GameObject defeatPanel;

        [Header("Inspector events")]
        [SerializeField] private UnityEvent onRunStarted = new();
        [SerializeField] private UnityEvent onPaused = new();
        [SerializeField] private UnityEvent onResumed = new();
        [SerializeField] private UnityEvent onWon = new();
        [SerializeField] private UnityEvent onLost = new();

        public event Action<GameFlowState> StateChanged;

        public GameFlowState State { get; private set; } = GameFlowState.Preparing;

        private void OnEnable()
        {
            if (playerHealth != null)
            {
                playerHealth.Died += LoseRun;
            }

            if (progressTracker != null)
            {
                progressTracker.Completed += WinRun;
            }
        }

        private void Start()
        {
            StartRun();
        }

        private void OnDisable()
        {
            if (playerHealth != null)
            {
                playerHealth.Died -= LoseRun;
            }

            if (progressTracker != null)
            {
                progressTracker.Completed -= WinRun;
            }

            if (State == GameFlowState.Paused)
            {
                Time.timeScale = 1f;
            }
        }

        public void StartRun()
        {
            Time.timeScale = 1f;
            SetPanels(false, false, false);
            playerHealth?.ResetHealth();
            progressTracker?.ResetProgress();
            SetGameplayEnabled(true);
            SetState(GameFlowState.Playing);
            onRunStarted.Invoke();
        }

        public void Pause()
        {
            if (State != GameFlowState.Playing)
            {
                return;
            }

            Time.timeScale = 0f;
            SetActive(pausePanel, true);
            SetState(GameFlowState.Paused);
            onPaused.Invoke();
        }

        public void Resume()
        {
            if (State != GameFlowState.Paused)
            {
                return;
            }

            Time.timeScale = 1f;
            SetActive(pausePanel, false);
            SetState(GameFlowState.Playing);
            onResumed.Invoke();
        }

        public void TogglePause()
        {
            if (State == GameFlowState.Paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        public void WinRun()
        {
            if (State != GameFlowState.Playing)
            {
                return;
            }

            EndRun(GameFlowState.Won, victoryPanel);
            onWon.Invoke();
        }

        public void LoseRun()
        {
            if (State != GameFlowState.Playing)
            {
                return;
            }

            EndRun(GameFlowState.Lost, defeatPanel);
            onLost.Invoke();
        }

        public void RestartScene()
        {
            Time.timeScale = 1f;
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }

        private void EndRun(GameFlowState endState, GameObject panel)
        {
            SetGameplayEnabled(false);
            SetActive(panel, true);
            SetState(endState);
        }

        private void SetGameplayEnabled(bool value)
        {
            if (gameplayBehaviours == null)
            {
                return;
            }

            foreach (Behaviour gameplayBehaviour in gameplayBehaviours)
            {
                if (gameplayBehaviour != null)
                {
                    gameplayBehaviour.enabled = value;
                }
            }
        }

        private void SetPanels(bool pause, bool victory, bool defeat)
        {
            SetActive(pausePanel, pause);
            SetActive(victoryPanel, victory);
            SetActive(defeatPanel, defeat);
        }

        private void SetState(GameFlowState state)
        {
            State = state;
            StateChanged?.Invoke(state);
        }

        private static void SetActive(GameObject target, bool value)
        {
            if (target != null)
            {
                target.SetActive(value);
            }
        }
    }
}
