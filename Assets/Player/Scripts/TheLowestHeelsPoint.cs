using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Scripts
{
    public class TheLowestHeelsPoint : MonoBehaviour
    {
        [FormerlySerializedAs("heelsSpawner")] [SerializeField] private HeelsManager heelsManager;
        private Transform lastHeelsTransform;
        private bool hasHeels;
        private void OnEnable()
        {
            heelsManager.OnHeelsUpdated += UpdateLastHeelsTransform;
        }

        private void Awake()
        {
            UpdateLastHeelsTransform();
        }

        private void Update()
        {
            if (hasHeels)
            {
              transform.localPosition =lastHeelsTransform.localPosition;
            }

        }

        private void UpdateLastHeelsTransform()
        {
            if (heelsManager.Heels.Count > 0)
            {
                hasHeels = true;
                lastHeelsTransform = heelsManager.Heels.Last().transform;
            }
            else
            {
                transform.localPosition = Vector3.zero;
                hasHeels = false;
            }
        }

        private void OnDisable()
        {
            heelsManager.OnHeelsUpdated -= UpdateLastHeelsTransform;
        }
    }
}
