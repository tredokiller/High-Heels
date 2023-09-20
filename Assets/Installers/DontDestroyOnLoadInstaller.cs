using Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class DontDestroyOnLoadInstaller : MonoInstaller
    {
        [SerializeField] private GameObject inputManager;

        public override void InstallBindings()
        {
            var inputManagerInstance = Instantiate(inputManager);
            DontDestroyOnLoad(inputManager);
            Container.Bind<InputManager>().FromComponentOn(inputManagerInstance).AsSingle();
        }
    }
}
