using System;
using Common.Tags;
using UnityEngine;

namespace Scenes.WinPlatform.MultiplierZone
{
    public class MultiplierZone : MonoBehaviour
    {
        [SerializeField, Range(1, 5)] private int multiplyValue;
        public static Action<int> OnMultiplierStand;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.PlayerTag))
            { 
                OnMultiplierStand.Invoke(multiplyValue);
            }
        }

    }
}
