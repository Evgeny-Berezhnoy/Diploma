using UnityEngine;
using Zenject;

public class ExplosionInitializer
{
    #region Injected methods

    [Inject]
    private void Initialize(
        // GameContextInjector
        [Inject] ControllersManager<EGameState> controllersManager,
        // ExplosionSettings
        [Inject(Id = "ExplosionSettings : BufferQuantity")] int spawnerBufferQuantity,
        [Inject(Id = "ExplosionSettings : Data")] ExplosionData data,
        // ExplosionInjector
        [Inject(Id = "ExplosionInjector : onExplosion")] ISubscriptionProperty<Transform> onExplosion,
        [Inject(Id = "ExplosionInjector : Pool")] Transform pool)
    {
        var exposiveViewRegistrator = new ExplosiveViewRegistrator(onExplosion);

        var poolData = new ExplosionPoolData(data, pool);

        var spawner = new ExplosionSpawner(poolData, spawnerBufferQuantity);

        var explosionService = new ExplosionService(spawner, onExplosion);

        controllersManager.AddController(exposiveViewRegistrator, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause, EGameState.Victory);
        controllersManager.AddController(explosionService, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause, EGameState.Victory);
    }

    #endregion
}
