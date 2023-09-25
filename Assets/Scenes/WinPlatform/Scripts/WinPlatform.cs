using System;
using Cinemachine;
using Managers;
using UnityEngine;
using Zenject;

namespace Scenes.WinPlatform.Scripts
{
    public class WinPlatform : MonoBehaviour
    {
        [SerializeField] private new CinemachineVirtualCamera camera;
        
        [Inject] private LevelManager _levelManager;
        
        private void Awake()
        {
            camera.enabled = false;
        }

        private void OnEnable()
        {
            _levelManager.OnLevelFullyCompleted += ChangeCamera;
        }

        private void ChangeCamera()
        {
            camera.enabled = true;
        }

        private void OnDisable()
        {
            _levelManager.OnLevelFullyCompleted -= ChangeCamera;
        }
    }
}
