using System;
using Common.Enums;
using UnityEngine;

namespace Heels.Scripts
{
    public class Heels : MonoBehaviour
    {
        [SerializeField] private Transform leftHeel;
        [SerializeField] private Transform rightHeel;

        public void SetPosition(Directions heel, Vector3 position)
        {
            if (heel == Directions.Left)
            {
                leftHeel.position = position;
                return;
            }
            rightHeel.position = position;
        }

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
