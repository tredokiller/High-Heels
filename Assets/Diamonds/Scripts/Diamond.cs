using System;
using Common.Tags;
using UnityEngine;

namespace Diamonds.Scripts
{
    public class Diamond : MonoBehaviour
    {
        public static Action OnDiamondCollected;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.PlayerTag))
            {
                OnDiamondCollected.Invoke();
                Destroy(gameObject);
            }
        }
    }
}
