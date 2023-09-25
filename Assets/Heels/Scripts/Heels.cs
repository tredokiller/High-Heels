using System;
using Player.Scripts;
using UnityEngine;
using Zenject;

namespace Heels.Scripts
{
    public class Heels : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private Transform leftHeel;
        [SerializeField] private Transform rightHeel;

        private const float DistanceToCheckObstacle = 0.8f;
        private bool canCheckObstacles;
        
        [Inject] private HeelsManager heelsManager;
        
        [SerializeField] private LayerMask heelsObstacleLayer;

        private void Awake()
        {
            canCheckObstacles = true;
        }

        private void Update()
        {
            if (canCheckObstacles)
            {
                CheckObstacle();
            }
        }

        private void CheckObstacle()
        {
            if (Physics.Raycast(transform.position, transform.forward, DistanceToCheckObstacle , heelsObstacleLayer))
            {
                TakeOffHeels();
                canCheckObstacles = false;
            }
        }

        private void TakeOffHeels()
        {
            heelsManager.TakeOffHeels(this);
        }
        
        public Transform GetLeftHeel()
        {
            return leftHeel;
        }
        
        public Transform GetRightHeel()
        {
            return rightHeel;
        }
        
        
    }
}
