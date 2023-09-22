using System;
using DG.Tweening;
using Managers;
using Player.Scripts.States;
using UnityEngine;
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
        [SerializeField] private TheLowestHeelsPoint theLowestHeelsPoint;
        [SerializeField] private LayerMask groundLayerMask;
        [SerializeField] private LayerMask obstacleLayerMask;

        [Header("RagDoll")]
        [SerializeField] private Rigidbody[] ragDollRigidbodies;
        private Collider[] ragDollColliders;
        
        public const float MaxSideDistanceToMove= 3.5f;

        [Header("Sensitivity")] 
        [SerializeField, Range(0, 150)] private float horizontalSensitivity = 0.1f;
        
        private StateMachine _stateMachine;
        private InputManager _inputManager;

        public HeelsSpawner HeelsSpawner { private set; get; }
        public InputMap.PlayerActions PlayerActions { private set; get; }
        public Vector2 InputPlayer { private set; get; }
        public Vector2 NonZeroInputPlayer { private set; get; }
        public Rigidbody Rb { private set; get; }
        public PlayerAnimationController PlayerAnimationController { private set; get; }
        public Animator PlayerAnimator { private set; get; }

        public Vector3 StartPositionForMovement { private set; get; }

        private IdleState idleState;
        private WalkState walkState;
        private DeathState deathState;

        private void Awake()
        {
            Rb = GetComponent<Rigidbody>();
            
            PlayerAnimator = GetComponent<Animator>();
            PlayerAnimationController = new PlayerAnimationController(this);

            _stateMachine = new StateMachine();

            idleState = new IdleState(this);
            walkState = new WalkState(this);
            deathState = new DeathState(this);
            
            GetCollidersFromRagDollRigidbodies();
            EnableRagDoll(false);
            
            _stateMachine.SetState(walkState);
        }

        [Inject]
        private void Constructor(InputManager inputManager, HeelsSpawner heelsSpawner)
        {
            _inputManager = inputManager ? inputManager : throw new ArgumentNullException(nameof(inputManager));
            HeelsSpawner = heelsSpawner ? heelsSpawner : throw new ArgumentNullException(nameof(heelsSpawner));
            PlayerActions = _inputManager.GetPlayerActions();
        }

        private void Start()
        {
            StartPositionForMovement = transform.position;
        }

        public void AddHeels()
        {
            transform.DOMoveY(transform.position.y + HeelsSpawner.DistanceBetweenHeels / 1.3f, 0.2f);
            HeelsSpawner.SpawnHeels();
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
            for (int i = 0; i < ragDollRigidbodies.Length; i++)
            {
                ragDollColliders[i].enabled = isEnabled;
                ragDollRigidbodies[i].isKinematic = !isEnabled;
            }
        }

        public void Move(Vector3 velocity, bool isClamped)
        {
            if (Physics.Raycast(theLowestHeelsPoint.transform.position, Vector3.down, 1f , groundLayerMask))
            {
                if (HeelsSpawner.Heels.Count > 0)
                {
                    Rb.constraints |= RigidbodyConstraints.FreezePositionY;
                }
            }
            else
            {
                Rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            }
            
            var newPosition = transform.position + velocity * Time.fixedDeltaTime;
            newPosition.x = Mathf.Clamp(newPosition.x, StartPositionForMovement.x - MaxSideDistanceToMove, 
                StartPositionForMovement.x + MaxSideDistanceToMove);
            
            Rb.MovePosition(newPosition);
        }
        
        private void Update()
        {
            InputPlayer = PlayerActions.TouchVelocity.ReadValue<Vector2>();
            if (InputPlayer != Vector2.zero)
            {
                NonZeroInputPlayer = InputPlayer;
            }
        }
        
        private void FixedUpdate()
        {
            _stateMachine.UpdateStateMachine();
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

        public LayerMask GetGroundLayerMask()
        {
            return groundLayerMask;
        }
    }
}
