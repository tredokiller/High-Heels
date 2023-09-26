using Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class DontDestroyOnLoadInstaller : MonoInstaller
    {
        [SerializeField] private GameObject inputManager;
        [SerializeField] private GameObject gameManager;

        public override void InstallBindings()
        {
            var inputManagerInstance = Instantiate(inputManager);
            var gameManagerInstance = Instantiate(gameManager);
            
            DontDestroyOnLoad(gameManagerInstance);
            DontDestroyOnLoad(inputManagerInstance);
            
            Container.Bind<InputManager>().FromComponentOn(inputManagerInstance).AsSingle();
            Container.Bind<GameManager>().FromComponentOn(gameManagerInstance).AsSingle();
        }
    }
}
