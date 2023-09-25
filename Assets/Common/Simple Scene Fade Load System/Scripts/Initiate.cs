using UnityEngine;
using UnityEngine.UI;

namespace Common.Simple_Scene_Fade_Load_System.Scripts
{
    public static class Initiate
    {
        static bool _areWeFading = false;

        //Create Fader object and assing the fade scripts and assign all the variables
        public static void Fade(string scene, Color col, float multiplier)
        {
            if (_areWeFading)
            {
                Debug.Log("Already Fading");
                return;
            }

            GameObject init = new GameObject();
            init.name = "Fader";
            Canvas myCanvas = init.AddComponent<Canvas>();
            myCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            init.AddComponent<Fader>();
            init.AddComponent<CanvasGroup>();
            init.AddComponent<Image>();

            Fader scr = init.GetComponent<Fader>();
            scr.fadeDamp = multiplier;
            scr.fadeScene = scene;
            scr.fadeColor = col;
            scr.start = true;
            _areWeFading = true;
            scr.InitiateFader();
        
        }

        public static void DoneFading() {
            _areWeFading = false;
        }
    }
}
