using Managers;
using Player.Scripts;
using UI.Scripts;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private GameObject heelsSpawner;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject levelManager;
        [SerializeField] private GameObject levelUIManager;
        public override void InstallBindings()
        {
            Container.Bind<HeelsManager>().FromComponentOn(heelsSpawner).AsSingle();
            Container.Bind<PlayerController>().FromComponentOn(player).AsSingle();
            Container.Bind<LevelManager>().FromComponentOn(levelManager).AsSingle();
            Container.Bind<LevelUIManager>().FromComponentOn(levelUIManager).AsSingle();
        }
    }
}
