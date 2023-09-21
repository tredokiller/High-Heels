using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Scripts
{
    public class HeelsSpawner : MonoBehaviour
    {
        public const float DistanceBetweenHeels = 1.5f;

        [SerializeField] private Transform leftFoot;
        [SerializeField] private Transform rightFoot;

        [SerializeField] private Transform leftFootTransformTranslator;
        [SerializeField] private Transform rightFootTransformTranslator;
        
        [SerializeField] private GameObject heelsPrefab;

        public List<Heels.Scripts.Heels> Heels { private set; get; }

        public Action OnHeelsUpdated;

        private void Awake()
        {
            Heels = new List<Heels.Scripts.Heels>();
        }

        private void Update()
        {
            leftFootTransformTranslator.position = leftFoot.position;
            rightFootTransformTranslator.position = rightFoot.position;

            leftFootTransformTranslator.localRotation = Quaternion.Euler(leftFoot.localRotation.eulerAngles.x, 0, leftFoot.localRotation.eulerAngles.z - 3);
            rightFootTransformTranslator.localRotation = Quaternion.Euler(rightFoot.localRotation.eulerAngles.x, 0, rightFoot.localRotation.eulerAngles.z + 3);
        }

        public void SpawnHeels()
        {
            var heelsPref = Instantiate(heelsPrefab, transform);
            
            var heels = heelsPref.GetComponent<Heels.Scripts.Heels>();
            
            var leftHeel = heels.GetLeftHeel();
            var rightHeel = heels.GetRightHeel();
            
            leftHeel.SetParent(leftFootTransformTranslator);
            rightHeel.SetParent(rightFootTransformTranslator);

            SetHeelsPositionRotation(leftHeel, rightHeel, out var heelsSpawnPosition);
            heelsPref.transform.localPosition = new Vector3(0,  heelsSpawnPosition.y,  0);
            
            Heels.Add(heels);
            
            OnHeelsUpdated?.Invoke();
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
    }
}
