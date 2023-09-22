using Player.Scripts;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private GameObject heelsSpawner;
        [SerializeField] private GameObject player;
        public override void InstallBindings()
        {
            Container.Bind<HeelsSpawner>().FromComponentOn(heelsSpawner).AsSingle();
            Container.Bind<PlayerController>().FromComponentOn(player).AsSingle();
        }
    }
}
