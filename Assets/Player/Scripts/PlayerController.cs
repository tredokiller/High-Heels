using System;
using DG.Tweening;
using Managers;
using Player.Scripts.States;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using StateMachine = States.StateMachine;
using IState = States.IState;

namespace Player.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(PlayerIKController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Main")] 
        [SerializeField] private float moveSpeed = 3;
        [SerializeField] private float twineMoveSpeed = 5;
        [SerializeField] private float stickMoveSpeed = 3;

        [SerializeField] private float maxRootRotationZAngleToFall = 35f;

        [SerializeField] private TheLowestHeelsPoint theLowestHeelsPoint;
        [SerializeField] private Transform middlePointOfPlayer;
        
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private LayerMask obstacleLayerMask;
        [SerializeField] private LayerMask interactionLayerMask;

        [Header("RagDoll")]
        [SerializeField] private Rigidbody[] ragDollRigidbodies;
        private Collider[] ragDollColliders;

        private const float MaxSideDistanceToMove= 3.5f;
        private const float MaxDistanceToCheckObject = 0.55f;
        public const float MaxAngleToFallOnTwin = 15f;

        public const float DurationChangePositionY = 0.2f;
        private const float DistanceToGround = 1.1f;

        [Header("Sensitivity")] 
        [SerializeField, Range(0, 150)] private float horizontalSensitivity = 0.1f;
        public const float HorizontalOnTwineSensitivityMultiplier = 0.5f;
        
        private StateMachine _stateMachine;
        private InputManager _inputManager;
        private LevelManager _levelManager;
        private HeelsManager HeelsManager { set; get; }
        private InputMap.PlayerActions PlayerActions { set; get; }
        public Vector2 InputPlayer { private set; get; }
        public Rigidbody Rb { private set; get; }
        public PlayerAnimationController PlayerAnimationController { private set; get; }
        public PlayerIKController PlayerIKController { private set; get; }
        public Animator PlayerAnimator { private set; get; }

        private Vector3 StartPositionForMovement { set; get; }

        private IState _idleState;
        private IState _walkState;
        private IState _deathState;
        private IState _winDanceState;
        private IState _winWalkState;
        private IState _twineState;
        private IState _stickWalkState;
        
        public PlayerStates CurrentPlayerStateType { private set; get; }

        public Action<PlayerStates> OnPlayerWon;

        public Action OnPlayerDied;
        
        public Action OnPlayerStickWalkingStarted;
        public Action OnPlayerStickWalkingFinished;

        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
            PlayerIKController = GetComponent<PlayerIKController>();

            PlayerAnimator = GetComponent<Animator>();
            PlayerAnimationController = new PlayerAnimationController(this);

            _stateMachine = new StateMachine();

            _idleState = new IdleState(this);
            _walkState = new WalkState(this , PlayerIKController);
            _deathState = new DeathState(this);
            _winDanceState = new WinDanceState(this);
            _winWalkState = new WinWalkState(this);
            _twineState = new TwineState(this);
            _stickWalkState = new StickWalkState(this, PlayerIKController);
            
            GetCollidersFromRagDollRigidbodies();
            EnableRagDoll(false);

            SetState(PlayerStates.Idling);
        }

        [Inject]
        private void Constructor(InputManager inputManager, HeelsManager heelsManager, LevelManager levelManager)
        {
            _inputManager = inputManager ? inputManager : throw new ArgumentNullException(nameof(inputManager));
            HeelsManager = heelsManager ? heelsManager : throw new ArgumentNullException(nameof(heelsManager));
            _levelManager = levelManager ? levelManager : throw new ArgumentNullException(nameof(levelManager));
            
            PlayerActions = _inputManager.GetPlayerActions();
        }

        private void Start()
        {
            StartPositionForMovement = transform.position;
        }

        private void OnEnable()
        {
            _levelManager.OnLevelStarted += () => SetState(PlayerStates.Walking);
        }

        public void AddHeels()
        {
            transform.DOMoveY(transform.position.y + HeelsManager.DistanceBetweenHeels, DurationChangePositionY);
            HeelsManager.SpawnHeels();
        }

        private void GetCollidersFromRagDollRigidbodies()
        {
            ragDollColliders = new Collider[ragDollRigidbodies.Length];
            for (int i = 0; i < ragDollRigidbodies.Length; i++)
            {
                ragDollColliders[i] = ragDollRigidbodies[i].GetComponent<Collider>();
            }
        }
        
        public void EnableRagDoll(bool isEnabled)
        {
            EnableRagDollColliders(isEnabled);
            for (int i = 0; i < ragDollRigidbodies.Length; i++)
            {
                ragDollRigidbodies[i].isKinematic = !isEnabled;
            }
        }

        private void EnableRagDollColliders(bool isEnabled)
        {
            for (int i = 0; i < ragDollRigidbodies.Length; i++)
            {
                ragDollColliders[i].enabled = isEnabled;
            }
        }

        public void Move(Vector3 velocity, bool isClamped)
        {
            CheckForwardObject();
            if (IsGrounded())
            {
                if (HeelsManager.Heels.Count > 0)
                { Rb.constraints |= RigidbodyConstraints.FreezePositionY;
                }
            }
            else
            {
               Rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            }

            if (Rb.velocity.y < -7f)
            {
                SetState(PlayerStates.Death);
            }
            
            if (isClamped)
            {
                var position = Rb.position + velocity * Time.fixedDeltaTime;
                position.x = Mathf.Clamp(position.x, StartPositionForMovement.x - MaxSideDistanceToMove, 
                    StartPositionForMovement.x + MaxSideDistanceToMove);

                Vector3 neededVelocity = (position - Rb.position) / Time.fixedDeltaTime;
                
                velocity = neededVelocity;
            }
            Rb.velocity = new Vector3(velocity.x, Rb.velocity.y, velocity.z);
            
        }

        private void CheckForwardObject()
        {
            var ray = new Ray(middlePointOfPlayer.position, Vector3.forward);

            if (Physics.Raycast(ray, MaxDistanceToCheckObject , obstacleLayerMask))
            {
                SetState(PlayerStates.Death);
            }

            if (Physics.Raycast(ray, MaxDistanceToCheckObject, interactionLayerMask))
            {
                SetState(PlayerStates.WinDancing);
            }
        }
        
        private void Update()
        {
            InputPlayer = PlayerActions.TouchVelocity.ReadValue<Vector2>();
        }

        public void SetState(PlayerStates state)
        {
            var previousState = CurrentPlayerStateType;
            CurrentPlayerStateType = state;
            switch (state)
            {
                case PlayerStates.Death:
                    _stateMachine.SetState(_deathState);
                    break;
                case PlayerStates.Walking:
                    _stateMachine.SetState(_walkState);
                    break;
                case PlayerStates.Idling:
                    _stateMachine.SetState(_idleState);
                    break;
                case PlayerStates.WinDancing:
                    _stateMachine.SetState(_winDanceState);
                    break;
                case PlayerStates.WinWalking:
                    _stateMachine.SetState(_winWalkState);
                    break;
                case PlayerStates.Twine:
                    _stateMachine.SetState(_twineState);
                    break;
                case PlayerStates.StickWalking:
                    _stateMachine.SetState(_stickWalkState);
                    break;
                default:
                    CurrentPlayerStateType = previousState;
                    break;
            }
        }

        private void OnDisable()
        {
            _levelManager.OnLevelStarted -= () => SetState(PlayerStates.Walking);
        }

        private void FixedUpdate()
        {
            _stateMachine.UpdateStateMachine();
        }

        public float GetMoveSpeed()
        {
            return moveSpeed;
        }

        public float GetTwineMoveSpeed()
        {
            return twineMoveSpeed;
        }

        public float GetStickMoveSpeed()
        {
            return stickMoveSpeed;
        }
        
        public float GetHorizontalSensitivity()
        {
            return horizontalSensitivity;
        }

        public float GetMaxRootRotationZAngleToFall()
        {
            return maxRootRotationZAngleToFall;
        }

        public bool IsGrounded()
        {
            return (Physics.Raycast(theLowestHeelsPoint.transform.position, Vector3.down, DistanceToGround, groundLayerMask));
        }
    }
}
