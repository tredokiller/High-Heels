using System.Linq;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            SetFrameRate();
        }

        private void SetFrameRate()
        {
            QualitySettings.vSyncCount = 0;
            
            Resolution[] refreshRate = Screen.resolutions;
            Application.targetFrameRate = (int)refreshRate.Last().refreshRateRatio.value;

        }
    }
}
