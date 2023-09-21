using UnityEngine;

namespace Heels.Scripts
{
    public class Heels : MonoBehaviour
    {
        [SerializeField] private Transform leftHeel;
        [SerializeField] private Transform rightHeel;

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
