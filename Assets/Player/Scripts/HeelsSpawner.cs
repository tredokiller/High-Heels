using System.Collections.Generic;
using UnityEngine;

namespace Player.Scripts
{
    public class HeelsSpawner : MonoBehaviour
    {
        private const float DistanceBetweenHeels = 1.5f;

        [SerializeField] private Transform leftFoot;
        [SerializeField] private Transform rightFoot;

        [SerializeField] private Transform leftFootTransformTranslator;
        [SerializeField] private Transform rightFootTransformTranslator;
        
        [SerializeField] private GameObject heelsPrefab;

        private List<Heels.Scripts.Heels> _heels;

        private void Start()
        {
            _heels = new List<Heels.Scripts.Heels>();
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
            leftHeel.localRotation = Quaternion.Euler(Vector3.zero);
            
            rightHeel.SetParent(rightFootTransformTranslator);
            rightHeel.localRotation = Quaternion.Euler(Vector3.zero);
            
            var heelsSpawnPosition = Vector3.down *_heels.Count * DistanceBetweenHeels;
            if (_heels.Count == 0)
            {
                heelsSpawnPosition = Vector3.down * 1 * (DistanceBetweenHeels / 2);
            }
            
            leftHeel.localPosition = heelsSpawnPosition;
            rightHeel.localPosition = heelsSpawnPosition;
            
            _heels.Add(heels);
        }
    }
}
