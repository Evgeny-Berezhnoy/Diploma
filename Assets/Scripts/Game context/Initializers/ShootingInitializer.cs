using Zenject;

public class ShootingInitializer 
{
    #region Injected methods

    [Inject]
    private void Initialize(
        // GameContextInjector
        [Inject] ControllersManager<EGameState> controllersManager,
        // ShootingSettings
        [Inject(Id = "ShootingSettings : hitContactsAmount")] int hitContactsAmount,
        [Inject(Id = "ShootingSettings : SpawnerHeatQuantity")] int spawnerHeatQuantity,
        [Inject(Id = "ShootingSettings : SpawnerBufferQuantity")] int spawnerBufferQuantity,
        // ShootingInjector
        [Inject(Id = "Shooting : onLaunch")] ISubscriptionProperty<ProjectileLaunchData> onLaunch,
        [Inject(Id = "Shooting : OnRemoteHit")] ISubscriptionMessenger<int, HealthController> onHit)
    {
        var projectileService =
            new ProjectileService(
                spawnerHeatQuantity,
                spawnerBufferQuantity,
                hitContactsAmount,
                onLaunch);

        var projectileViewRegistrator = new ProjectileViewRegistrator(onHit);

        controllersManager.AddController(projectileService, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause, EGameState.Victory);
        controllersManager.AddController(projectileViewRegistrator, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause, EGameState.Victory);
    }

    #endregion
}
