using System;
using DG.Tweening;
using Managers;
using Player.Scripts.States;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
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
        [FormerlySerializedAs("theLowestPoint")] [SerializeField] private TheLowestHeelsPoint theLowestHeelsPoint;

        public const float MaxDistanceToFallZone = 1f;
        
        [Header("Sensitivity")] 
        [SerializeField, Range(0, 15)] private float horizontalSensitivity = 0.1f;
        
        private StateMachine _stateMachine;

        private InputManager _inputManager;
        public InputMap.PlayerActions PlayerActions { private set; get; }
        public Vector2 InputPlayer { private set; get; }
        public Rigidbody Rb { private set; get; }
        public PlayerAnimationController PlayerAnimationController { private set; get; }
        public Animator PlayerAnimator { private set; get; }

        private IdleState idleState;
        private WalkState walkState;


        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
            
            PlayerAnimator = GetComponent<Animator>();
            PlayerAnimationController = new PlayerAnimationController(this);
            
            _stateMachine = new StateMachine();

            idleState = new IdleState(this);
            walkState = new WalkState(this);

            _stateMachine.SetState(walkState);
        }

        [Inject]
        private void Constructor(InputManager inputManager)
        {
            _inputManager = inputManager ? inputManager : throw new ArgumentNullException(nameof(inputManager));
            PlayerActions = _inputManager.GetPlayerActions();
        }

        private void OnEnable()
        {
            _inputManager.GetPlayerActions().TestButton.started += SpawnHeels;
        }

        private void SpawnHeels(InputAction.CallbackContext obj)
        {
            transform.DOMoveY(transform.position.y + HeelsSpawner.DistanceBetweenHeels, 0.3f);
            heelsSpawner.SpawnHeels();
        }

        private void Update()
        {
            InputPlayer = PlayerActions.TouchVelocity.ReadValue<Vector2>();
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

        public float GetHorizontalSensitivity()
        {
            return horizontalSensitivity;
        }

        public Transform GetTheLowestPoint()
        {
            return theLowestHeelsPoint.transform;
        }
    }
}
