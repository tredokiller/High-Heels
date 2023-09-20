using System;
using UnityEngine;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        private InputMap _inputMap;
        private InputMap.PlayerActions _playerActions;
        
        private void Awake()
        {
            _inputMap = new InputMap();
            _playerActions = _inputMap.Player;
        }

        private void OnEnable()
        {
           _inputMap.Enable(); 
        }

        public InputMap.PlayerActions GetPlayerActions()
        {
            return _playerActions;
        }
        
        private void OnDisable()
        {
            _inputMap.Disable(); 
        }
    }
}
