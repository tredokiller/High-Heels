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
            Container.Bind<InputManager>().FromComponentInNewPrefab(inputManager).UnderTransform(transform).AsSingle()
                .NonLazy();
            Container.Bind<GameManager>().FromComponentInNewPrefab(gameManager).UnderTransform(transform).AsSingle()
                .NonLazy();
        }
    }
}
