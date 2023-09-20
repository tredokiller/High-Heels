using System;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Zenject;
using StateMachine = States.StateMachine;

namespace Player.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Main")] 
        [SerializeField] private float moveSpeed = 3;
        [SerializeField] private HeelsSpawner heelsSpawner;
        
        public Rigidbody Rb { private set; get; }
        
        public PlayerAnimationController PlayerAnimationController { private set; get; }
        public Animator PlayerAnimator { private set; get; }

        private StateMachine _stateMachine;

        [Inject] private InputManager _inputManager;


        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
            
            PlayerAnimator = GetComponent<Animator>();
            PlayerAnimationController = new PlayerAnimationController(this);
            
            _stateMachine = new StateMachine();
        }

        private void OnEnable()
        {
            _inputManager.GetPlayerActions().TestButton.started += SpawnHeels;
        }

        private void SpawnHeels(InputAction.CallbackContext obj)
        {
            transform.DOMoveY(transform.position.y + 2f, 0.3f);
            heelsSpawner.SpawnHeels();
        }
        
        void FixedUpdate()
        {
            _stateMachine.UpdateStateMachine();
        }
        
        private void OnDisable()
        {
            _inputManager.GetPlayerActions().TestButton.started -= SpawnHeels;
        }

        public float GetMoveSpeed()
        {
            return moveSpeed;
        }
    }
}
