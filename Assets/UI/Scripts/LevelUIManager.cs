using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Scripts
{
    public class LevelUIManager : MonoBehaviour
    {
        [Inject] private LevelManager _levelManager;
        [SerializeField] private RectTransform tapToStartMenu;
        [SerializeField] private Button nextLevelButton;
        [SerializeField] private Button restartLevelButton;

        public Action OnNextLevelButtonPressed;
        public Action OnRestartLevelButtonPressed;
        
        private void Awake()
        {
            nextLevelButton.onClick.AddListener(ToNextLevelButtonPressed);
            restartLevelButton.onClick.AddListener(RestartLevelButtonPressed);

            SetVisibleTapToStartMenu(true);
            SetVisibleNextLevelMenu(false);
            SetVisibleRestartLevelMenu(false);
        }

        private void OnEnable()
        {
            _levelManager.OnLevelStarted += () => SetVisibleTapToStartMenu(false);
            _levelManager.OnLevelCompleted += () => SetVisibleNextLevelMenu(true);
            _levelManager.OnLevelFailed += () => SetVisibleRestartLevelMenu(true);
        }

        private void SetVisibleTapToStartMenu(bool isVisible)
        {
            tapToStartMenu.gameObject.SetActive(isVisible);
        }

        private void SetVisibleNextLevelMenu(bool isVisible)
        {
            nextLevelButton.gameObject.SetActive(isVisible);
        }

        private void SetVisibleRestartLevelMenu(bool isVisible)
        {
            restartLevelButton.gameObject.SetActive(isVisible);
        }

        private void OnDisable()
        {
            restartLevelButton.onClick.RemoveListener(RestartLevelButtonPressed);
            nextLevelButton.onClick.RemoveListener(ToNextLevelButtonPressed);
            
            _levelManager.OnLevelStarted -= () => SetVisibleTapToStartMenu(false);
            _levelManager.OnLevelCompleted -= () => SetVisibleNextLevelMenu(true);
            _levelManager.OnLevelFailed -= () => SetVisibleRestartLevelMenu(true);
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
