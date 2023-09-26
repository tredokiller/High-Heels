using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Player.Scripts
{
    public class PlayerIKController : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private PlayerController playerController;
        
        [SerializeField] private Transform spineRotator;
        [SerializeField] private Transform rootRotator;
        
        [SerializeField] private Rig rig;

        private const float SmoothLerpValue = 3f;
        private const float AngleZMultiplier = 4f;

        private void OnEnable()
        {
            playerController.OnPlayerDied += () => rig.weight = 0;
        }

        public void UpdateSpineRotationZ(float currentZVelocity)
        {
            var rotation = spineRotator.rotation.eulerAngles;
            rotation.z = Mathf.LerpAngle(rotation.z, currentZVelocity * AngleZMultiplier, SmoothLerpValue * Time.deltaTime);
            
            spineRotator.transform.rotation = Quaternion.Euler(rotation);
        }

        public void UpdateRootRotationZ(float currentZVelocity)
        {
            var rotation = rootRotator.rotation.eulerAngles;
            rotation.z = currentZVelocity;  
            
            rootRotator.rotation = Quaternion.Euler(rotation);
        }

        public float GetCurrentRootRotatorZAngle()
        {
            return rootRotator.rotation.eulerAngles.z;
        }
        
        private void OnDisable()
        {
            playerController.OnPlayerDied -= () => rig.weight = 0;
        }
    }
}
