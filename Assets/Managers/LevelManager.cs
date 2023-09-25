using System;
using Player.Scripts;
using Scenes.Folder;
using UI.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        [Inject] private PlayerController _playerController;
        [Inject] private InputManager _inputManager;

        [Inject] private LevelUIManager _levelUIManager;

        [SerializeField] private Levels level;

        private bool _isLevelStarted; 

        public Action OnLevelFullyCompleted;
        public Action OnLevelCompleted;
        public Action OnLevelFailed;
        public Action OnLevelStarted;
        
        public static  Action<Levels> OnLevelLoaded;

        public static Action OnLevelRestart;
        public static Action OnLevelExit;
        
        private void Awake()
        {
            OnLevelLoaded.Invoke(level);
        }

        private void OnEnable()
        {
            _inputManager.GetPlayerActions().TouchVelocity.started += ScreenTouched;
            _playerController.OnPlayerWon += CompleteLevel;
            _playerController.OnPlayerDied += LooseLevel;
            
            _levelUIManager.OnNextLevelButtonPressed += ExitLevel;
            _levelUIManager.OnRestartLevelButtonPressed += RestartLevel;

        }

        private void ScreenTouched(InputAction.CallbackContext obj)
        {
            StartLevel();
        }

        private void CompleteLevel(PlayerStates state)
        {
            if (state == PlayerStates.WinWalking)
            {
                OnLevelFullyCompleted?.Invoke();
            }
            OnLevelCompleted?.Invoke();
        }

        private void StartLevel()
        {
            if (_isLevelStarted)
            {
                return;
            }

            _isLevelStarted = true;
            OnLevelStarted.Invoke();
        }

        private void LooseLevel()
        {
            OnLevelFailed?.Invoke();
        }

        private void ExitLevel()
        {
            OnLevelExit.Invoke();
        }

        private void RestartLevel()
        {
            OnLevelRestart.Invoke();
        }
        
        private void OnDisable()
        {
            _inputManager.GetPlayerActions().TouchVelocity.started -= ScreenTouched;
            _playerController.OnPlayerWon -= CompleteLevel;
            _playerController.OnPlayerDied -= LooseLevel;
            
            _levelUIManager.OnNextLevelButtonPressed -= ExitLevel;
            _levelUIManager.OnRestartLevelButtonPressed -= RestartLevel;
        }
    }
}
