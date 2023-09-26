using System;
using System.Linq;
using Common.Simple_Scene_Fade_Load_System.Scripts;
using ModestTree;
using Scenes.Folder;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Levels[] levels;
        private Levels currentLevel;
        
        private const float TransitionBetweenLevelsDuration = 0.8f;
        
        private void Awake()
        {
            SetFrameRate();
        }

        private void OnEnable()
        {
            LevelManager.OnLevelLoaded += UpdateCurrentLevel;
            LevelManager.OnLevelExit += NextLevel;
            LevelManager.OnLevelRestart += () => LoadLevel(currentLevel);
        }

        private void UpdateCurrentLevel(Levels level)
        {
            currentLevel = level;
        }

        private void NextLevel()
        {
            if (currentLevel != levels.Last())
            {
                LoadLevel(levels[levels.IndexOf(currentLevel) + 1]);
            }
            else
            {
                LoadLevel(levels[0]);
            }
        }
        
        private void LoadLevel(Levels level)
        {
            Initiate.Fade(level.ToString() , Color.black, TransitionBetweenLevelsDuration);
        }

        private void SetFrameRate()
        {
            QualitySettings.vSyncCount = 0;
            
            Resolution[] refreshRate = Screen.resolutions;
            Application.targetFrameRate = (int)refreshRate.Last().refreshRateRatio.value;
        }

        private void OnDisable()
        {
            LevelManager.OnLevelLoaded -= UpdateCurrentLevel;
            LevelManager.OnLevelExit -= NextLevel;
            LevelManager.OnLevelRestart -= () => LoadLevel(currentLevel);
        }
    }
}
