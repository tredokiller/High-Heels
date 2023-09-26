using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Player.Scripts
{
    public class HeelsManager : MonoBehaviour
    {
        public const float DistanceBetweenHeels = 1.7f;

        [SerializeField] private Transform leftFoot;
        [SerializeField] private Transform rightFoot;

        [SerializeField] private Transform leftThigh;
        [SerializeField] private Transform rightThigh;

        [SerializeField] private Transform leftFootTransformTranslator;
        [SerializeField] private Transform rightFootTransformTranslator;
        
        [SerializeField] private GameObject heelsPrefab;

        private const float SmoothRotationZValue = 8f;

        [Inject] private DiContainer _diContainer;
        [Inject] private PlayerController _playerController;
        
        public List<Heels.Scripts.Heels> Heels { private set; get; }

        public Action OnHeelsUpdated;

        private void Awake()
        {
            Heels = new List<Heels.Scripts.Heels>();
        }

        private void LateUpdate()
        {
            UpdateHeelTranslatorTransforms();
        }

        private void UpdateHeelTranslatorTransforms()
        {
            leftFootTransformTranslator.position = leftFoot.position;
            rightFootTransformTranslator.position = rightFoot.position;

            leftFootTransformTranslator.localRotation = Quaternion.Euler(leftFoot.localRotation.eulerAngles.x  + 165, 0, leftThigh.rotation.eulerAngles.z + 3);
            rightFootTransformTranslator.localRotation = Quaternion.Euler(rightFoot.localRotation.eulerAngles.x + 165, 0, rightThigh.rotation.eulerAngles.z - 3);
        }

        public void SpawnHeels()
        {
            var heelsPref = _diContainer.InstantiatePrefab(heelsPrefab, transform);
            
            var heels = heelsPref.GetComponent<Heels.Scripts.Heels>();
            
            var leftHeel = heels.GetLeftHeel();
            var rightHeel = heels.GetRightHeel();
            
            leftHeel.SetParent(leftFootTransformTranslator);
            rightHeel.SetParent(rightFootTransformTranslator);

            SetHeelsPositionRotation(leftHeel, rightHeel, out var heelsSpawnPosition);
            heelsPref.transform.localPosition = new Vector3(0,  heelsSpawnPosition.y,  0);
            
            Heels.Add(heels);
            
            OnHeelsUpdated.Invoke();
        }

        private void SetHeelsPositionRotation(Transform leftHeel, Transform rightHeel ,out Vector3 heelsSpawnPosition)
        {
            heelsSpawnPosition = Vector3.down *Heels.Count * (DistanceBetweenHeels);
            if (Heels.Count == 0)
            {
                heelsSpawnPosition = Vector3.down * 1 * (DistanceBetweenHeels / 2);
            }
            else if (Heels.Count == 1)
            {
                heelsSpawnPosition = Vector3.down *Heels.Count * (DistanceBetweenHeels + DistanceBetweenHeels/2);
            }

            leftHeel.transform.localPosition = heelsSpawnPosition;
            rightHeel.transform.localPosition = heelsSpawnPosition;
            
            leftHeel.localRotation = Quaternion.Euler(Vector3.zero);
            rightHeel.localRotation = Quaternion.Euler(Vector3.zero);
        }

        public void TakeOffHeels(Heels.Scripts.Heels heels)
        {
            if (Heels.Contains(heels))
            {
                Heels.Remove(heels);
                
                heels.GetLeftHeel().SetParent(heels.transform);
                heels.GetRightHeel().SetParent(heels.transform);
                heels.transform.SetParent(null);
                
                OnHeelsUpdated.Invoke();
            }
        }
    }
}
