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
        
        public event Action OnNextLevelButtonPressed;
        public event Action OnRestartLevelButtonPressed;

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
            
            HideRestartLevelMenu();
            HideNextLevelMenu();
            
            ShowStickMenu();
            ShowTapToStartMenu();

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
            _playerController.OnPlayerStickWalkingStarted  += ShowStickMenu;
            _playerController.OnPlayerStickWalkingFinished += HideStickMenu;
            _levelManager.OnLevelStarted +=  HideTapToStartMenu;
            _levelManager.OnLevelCompleted += ShowNextLevelMenu;
            _levelManager.OnLevelFailed += ShowRestartLevelMenu;
            _levelManager.OnDiamondsUpdated += UpdateCountOfDiamonds;
        }

        private void HideTapToStartMenu()
        {
            tapToStartMenu.gameObject.SetActive(false);
        }

        private void ShowTapToStartMenu()
        {
            tapToStartMenu.gameObject.SetActive(true);
        }
        
        private void HideStickMenu()
        {
            _isStickMenuOpened = true;
            stickMenu.gameObject.SetActive(true);
        }
        
        private void ShowStickMenu()
        {
            _isStickMenuOpened = false;
            stickMenu.gameObject.SetActive(false);
        }
        
        

        private void UpdateStickMenuArrow()
        {
            var rotation = stickArrow.localRotation.eulerAngles;
            rotation.z = _playerIKController.GetCurrentRootRotatorZAngle();
            stickArrow.eulerAngles = rotation / 2;
        }
        

        private void ShowNextLevelMenu()
        {
            nextLevelButton.gameObject.SetActive(true);
        }

        private void HideNextLevelMenu()
        {
            nextLevelButton.gameObject.SetActive(false);
        }

        private void ShowRestartLevelMenu()
        {
            restartLevelButton.gameObject.SetActive(true);
        }

        private void HideRestartLevelMenu()
        {
            restartLevelButton.gameObject.SetActive(false);
        }

        private void UpdateCountOfDiamonds()
        {
            diamondsCountText.text = _levelManager.MultipliedDiamonds.ToString();
        }
        
        private void OnDisable()
        {
            restartLevelButton.onClick.RemoveListener(RestartLevelButtonPressed);
            nextLevelButton.onClick.RemoveListener(ToNextLevelButtonPressed);
            
            _playerController.OnPlayerStickWalkingStarted  -= ShowStickMenu;
            _playerController.OnPlayerStickWalkingFinished -= HideStickMenu;
            _levelManager.OnLevelStarted -= HideTapToStartMenu;
            _levelManager.OnLevelCompleted -= ShowNextLevelMenu;
            _levelManager.OnLevelFailed -= ShowRestartLevelMenu;
            _levelManager.OnDiamondsUpdated -= UpdateCountOfDiamonds;
        }

        private void ToNextLevelButtonPressed()
        {
            OnNextLevelButtonPressed?.Invoke();
        }

        private void RestartLevelButtonPressed()
        {
            OnRestartLevelButtonPressed?.Invoke();
        }
    }
}
