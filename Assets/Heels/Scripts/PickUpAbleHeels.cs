using Common.Tags;
using Player.Scripts;
using UnityEngine;
using Zenject;

namespace Heels.Scripts
{
    public class PickUpAbleHeels : MonoBehaviour
    {
        [Inject] private PlayerController playerController;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.PlayerTag))
            {
                playerController.AddHeels();
                Destroy(gameObject);
            }
        }
    }
}
