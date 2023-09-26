using System;
using Managers;
using Player.Scripts;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Scripts
{
    public class LevelUIManager : MonoBehaviour
    {
        private LevelManager _levelManager;
        private PlayerController _playerController;

        private PlayerIKController _playerIKController;
        
        [SerializeField] private RectTransform tapToStartMenu;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button restartLevelButton;

        [SerializeField] private TextMeshProUGUI levelInfoText;
        [SerializeField] private TextMeshProUGUI diamondsCountText;

        [SerializeField] private RectTransform stickMenu;
        [SerializeField] private RectTransform stickArrow;
        
        public Action OnNextLevelButtonPressed;
        public Action OnRestartLevelButtonPressed;

        private const string LevelText = "Level ";
        private bool _isStickMenuOpened;

        [Inject]
        public void Constructor(LevelManager levelManager, PlayerController playerController)
        {
            _levelManager = levelManager ? levelManager : throw new ArgumentNullException(nameof(levelManager));
            _playerController = playerController ? playerController : throw new ArgumentNullException(nameof(playerController));
        }
        
        private void Awake()
        {
            _playerIKController = _playerController.PlayerIKController;            
            
            nextLevelButton.onClick.AddListener(ToNextLevelButtonPressed);
            restartLevelButton.onClick.AddListener(RestartLevelButtonPressed);

            SetVisibleTapToStartMenu(true);
            SetVisibleNextLevelMenu(false);
            SetVisibleRestartLevelMenu(false);
            SetVisibleStickMenu(false);

            levelInfoText.text = LevelText + ((int)_levelManager.GetLevel() + 1);
            UpdateCountOfDiamonds();
        }

        private void Update()
        {
            if (_isStickMenuOpened)
            {
                UpdateStickMenuArrow();
            }
        }

        private void OnEnable()
        {
            _playerController.OnPlayerStickWalkingStarted += () => SetVisibleStickMenu(true);
            _playerController.OnPlayerStickWalkingFinished += () => SetVisibleStickMenu(false);
            _levelManager.OnLevelStarted += () => SetVisibleTapToStartMenu(false);
            _levelManager.OnLevelCompleted += () => SetVisibleNextLevelMenu(true);
            _levelManager.OnLevelFailed += () => SetVisibleRestartLevelMenu(true);
            _levelManager.OnDiamondsUpdated += UpdateCountOfDiamonds;
        }

        private void SetVisibleTapToStartMenu(bool isVisible)
        {
            tapToStartMenu.gameObject.SetActive(isVisible);
        }

        private void SetVisibleStickMenu(bool isVisible)
        {
            _isStickMenuOpened = isVisible;
            stickMenu.gameObject.SetActive(isVisible);
        }

        private void UpdateStickMenuArrow()
        {
            var rotation = stickArrow.localRotation.eulerAngles;
            rotation.z = _playerIKController.GetCurrentRootRotatorZAngle();
            stickArrow.eulerAngles = rotation / 2;
        }
        

        private void SetVisibleNextLevelMenu(bool isVisible)
        {
            nextLevelButton.gameObject.SetActive(isVisible);
        }

        private void SetVisibleRestartLevelMenu(bool isVisible)
        {
            restartLevelButton.gameObject.SetActive(isVisible);
        }

        private void UpdateCountOfDiamonds()
        {
            diamondsCountText.text = _levelManager.MultipliedDiamonds.ToString();
        }
        
        private void OnDisable()
        {
            restartLevelButton.onClick.RemoveListener(RestartLevelButtonPressed);
            nextLevelButton.onClick.RemoveListener(ToNextLevelButtonPressed);
            
            _playerController.OnPlayerStickWalkingStarted -= () => SetVisibleStickMenu(true);
            _playerController.OnPlayerStickWalkingFinished -= () => SetVisibleStickMenu(false);
            _levelManager.OnLevelStarted -= () => SetVisibleTapToStartMenu(false);
            _levelManager.OnLevelCompleted -= () => SetVisibleNextLevelMenu(true);
            _levelManager.OnLevelFailed -= () => SetVisibleRestartLevelMenu(true);
            _levelManager.OnDiamondsUpdated -= UpdateCountOfDiamonds;
        }

        private void ToNextLevelButtonPressed()
        {
            OnNextLevelButtonPressed.Invoke();
        }

        private void RestartLevelButtonPressed()
        {
            OnRestartLevelButtonPressed.Invoke();
        }
    }
}
