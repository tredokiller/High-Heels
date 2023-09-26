using Common.Tags;
using Player.Scripts;
using UnityEngine;
using Zenject;

namespace Scenes.Zones.Scripts
{
    [RequireComponent(typeof(BoxCollider))]
    public class Zone : MonoBehaviour
    {
        [Inject] private PlayerController playerController;
        [SerializeField] private ZoneTypes zoneType;

        private const float DefaultZoneSizeX = 4f;
        private const float DefaultZoneSizeY = 2f;
        private const float DefaultZoneSizeZ = 0.5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.PlayerTag))
            {
                Interact();
            }
        }

        private void Interact()
        {
            if (playerController.CurrentPlayerStateType == PlayerStates.Death)
            {
                return;
            }
            switch (zoneType)
            {
                case ZoneTypes.Walk:
                    playerController.SetState(PlayerStates.Walking);
                    break;
                case ZoneTypes.AbsoluteWin:
                    playerController.SetState(PlayerStates.WinWalking);
                    break;
                case ZoneTypes.Twine:
                    playerController.SetState(PlayerStates.Twine);
                    break;
                case ZoneTypes.Stick:
                    playerController.SetState(PlayerStates.StickWalking);
                    break;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(transform.position , new Vector3(DefaultZoneSizeX, DefaultZoneSizeY, DefaultZoneSizeZ));
        }
    }
}
