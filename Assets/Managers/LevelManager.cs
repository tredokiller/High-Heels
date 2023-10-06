using System;
using Diamonds.Scripts;
using Player.Scripts;
using Scenes.Folder;
using Scenes.WinPlatform.MultiplierZone;
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

        public event Action OnLevelFullyCompleted;
        public event Action OnLevelCompleted;
        public event Action OnLevelFailed;
        public event Action OnLevelStarted;
        public event Action OnDiamondsUpdated;

        public static event Action<Levels> OnLevelLoaded;

        public static event Action OnLevelRestart;
        public static event Action OnLevelExit;

        public int CollectedDiamonds { private set; get; }
        public int MultipliedDiamonds { private set; get; }
        
        private void Awake()
        {
            OnLevelLoaded?.Invoke(level);
        }

        private void OnEnable()
        {
            _inputManager.GetPlayerActions().TouchVelocity.started += ScreenTouched;
            _playerController.OnPlayerWon += CompleteLevel;
            _playerController.OnPlayerDied += LooseLevel;
            
            _levelUIManager.OnNextLevelButtonPressed += ExitLevel;
            _levelUIManager.OnRestartLevelButtonPressed += RestartLevel;
            MultiplierZone.OnMultiplierStand += MultiplyDiamonds; 

            Diamond.OnDiamondCollected += AddDiamond;

        }

        private void ScreenTouched(InputAction.CallbackContext obj)
        {
            StartLevel();
        }

        private void AddDiamond()
        {
            CollectedDiamonds += 1;
            MultipliedDiamonds = CollectedDiamonds;
            OnDiamondsUpdated?.Invoke();
        }

        private void MultiplyDiamonds(int multiplier)
        {
            MultipliedDiamonds = CollectedDiamonds * multiplier;
            OnDiamondsUpdated?.Invoke();
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
            OnLevelStarted?.Invoke();
        }

        private void LooseLevel()
        {
            OnLevelFailed?.Invoke();
        }

        private void ExitLevel()
        {
            OnLevelExit?.Invoke();
        }

        private void RestartLevel()
        {
            OnLevelRestart?.Invoke();
        }

        public Levels GetLevel()
        {
            return level;
        }
        
        private void OnDisable()
        {
            _inputManager.GetPlayerActions().TouchVelocity.started -= ScreenTouched;
            _playerController.OnPlayerWon -= CompleteLevel;
            _playerController.OnPlayerDied -= LooseLevel;
            
            _levelUIManager.OnNextLevelButtonPressed -= ExitLevel;
            _levelUIManager.OnRestartLevelButtonPressed -= RestartLevel;
            Diamond.OnDiamondCollected -= AddDiamond;
        }
    }
}
