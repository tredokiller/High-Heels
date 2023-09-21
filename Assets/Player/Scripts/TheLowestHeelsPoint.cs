using System.Linq;
using UnityEngine;

namespace Player.Scripts
{
    public class TheLowestHeelsPoint : MonoBehaviour
    {
        [SerializeField] private HeelsSpawner heelsSpawner;
        private Transform lastHeelsTransform;
        private bool hasHeels;
        private void OnEnable()
        {
            heelsSpawner.OnHeelsUpdated += UpdateLastHeelsTransform;
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
                return;
            }

            transform.localPosition = Vector3.zero;

        }

        private void UpdateLastHeelsTransform()
        {
            if (heelsSpawner.Heels.Count > 0)
            {
                hasHeels = true;
                lastHeelsTransform = heelsSpawner.Heels.Last().transform;
            }
            else
            {
                hasHeels = false;
            }
        }

        private void OnDisable()
        {
            heelsSpawner.OnHeelsUpdated -= UpdateLastHeelsTransform;
        }
    }
}
