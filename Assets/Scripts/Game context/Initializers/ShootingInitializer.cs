using Zenject;

public class ShootingInitializer 
{
    #region Injected methods

    [Inject]
    private void Initialize(
        // GameContextInjector
        [Inject(Id = "GameContext : onRetry")] ISubscriptionProperty onRetry,
        [Inject] ControllersManager<EGameState> controllersManager,
        // ShootingSettings
        [Inject(Id = "ShootingSettings : hitContactsAmount")] int hitContactsAmount,
        [Inject(Id = "ShootingSettings : SpawnerHeatQuantity")] int spawnerHeatQuantity,
        [Inject(Id = "ShootingSettings : SpawnerBufferQuantity")] int spawnerBufferQuantity,
        // ShootingInjector
        [Inject(Id = "Shooting : onAddController")] ISubscriptionProperty<ProjectileController> onAddController,
        [Inject(Id = "Shooting : onRemoveController")] ISubscriptionProperty<ProjectileController> onRemoveController,
        [Inject(Id = "Shooting : onLaunch")] ISubscriptionProperty<ProjectileLaunchData> onLaunch,
        [Inject(Id = "Shooting : OnRemoteHit")] ISubscriptionMessenger<int, HealthController> onHit)
    {
        var projectileService =
            new ProjectileService(
                spawnerHeatQuantity,
                spawnerBufferQuantity,
                hitContactsAmount,
                onAddController,
                onRemoveController,
                onLaunch);

        var moveService     = new ProjectileMoveService();
        var physicsService  = new ProjectilePhysicsService();

        var projectileViewRegistrator = new ProjectileViewRegistrator(onHit);

        onRetry.Subscribe(projectileService.Clear);

        onAddController.Subscribe(moveService.OnAddController);
        onAddController.Subscribe(physicsService.OnAddController);

        onRemoveController.Subscribe(moveService.OnRemoveController);
        onRemoveController.Subscribe(physicsService.OnRemoveController);
        
        controllersManager.AddController(projectileService, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause, EGameState.Victory);
        controllersManager.AddController(moveService, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause, EGameState.Victory);
        controllersManager.AddController(physicsService, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause);
        controllersManager.AddController(projectileViewRegistrator, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause, EGameState.Victory);
    }

    #endregion
}
