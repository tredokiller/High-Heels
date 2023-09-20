using System;
using System.Collections.Generic;
using Common.Enums;
using UnityEngine;

namespace Player.Scripts
{
    public class HeelsSpawner : MonoBehaviour
    {
        private const float VerticalDistanceBetweenHeels = 1.6f;
        
        [SerializeField] private Transform leftFoot;
        [SerializeField] private Transform rightFoot;

        [SerializeField] private Transform leftFootTransformTranslator;
        [SerializeField] private Transform rightFootTransformTranslator;
        
        [SerializeField] private GameObject heelsPrefab;

        private List<Heels.Scripts.Heels> _heels;

        private void Start()
        {
            _heels = new List<Heels.Scripts.Heels>();
            SpawnHeels();
        }

        private void Update()
        {
            leftFootTransformTranslator.position = leftFoot.position;
            rightFootTransformTranslator.position = rightFoot.position;
            if (_heels.Count >0)
            {
                for (int i = 0; i < _heels.Count; i++)
                {
                    _heels[i].SetPosition(Directions.Left , leftFoot.position + Vector3.down * (VerticalDistanceBetweenHeels * i));
                    _heels[i].SetPosition(Directions.Right , rightFoot.position + Vector3.down * (VerticalDistanceBetweenHeels * i));
                }
            }
        }

        public void SpawnHeels()
        {
            var heelsPref = Instantiate(heelsPrefab, transform);
            var heels = heelsPref.GetComponent<Heels.Scripts.Heels>();
            _heels.Add(heels);
            
        }
    }
}
