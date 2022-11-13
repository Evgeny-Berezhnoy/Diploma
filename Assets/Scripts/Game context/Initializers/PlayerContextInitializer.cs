using UnityEngine;
using Zenject;

public class PlayerContextInitializer
{
    #region Injected methods

    [Inject]
    public void Initialize(
        // AbstractionsInjector
        [Inject] Disposer disposer,
        // PlayerSettings
        [Inject(Id = "PlayerSettings : ResurrectionContactsAmount")] int resurrectionContactsAmount,
        [Inject(Id = "PlayerSettings : ResurrectionViewPrefab")] string resurrectionViewPrefab,
        [Inject(Id = "PlayerSettings : SentryService")] string sentryServicePrefab,
        // PlayerContextInjector
        [Inject(Id = "PlayerContext : onResurrectionContact")] ISubscriptionProperty<Collider2D> onResurrectionContact,
        [Inject(Id = "PlayerContext : onCheckResurrectNecessity")] ISubscriptionProperty<bool> onCheckResurrectNecessity,
        [Inject(Id = "PlayerContext : onPlayerHealthChanged")] ISubscriptionProperty<float> onPlayerHealthChanged,
        // GameContextInjector
        [Inject(Id = "GameContext : MainCamera")] Camera camera,
        [Inject(Id = "GameContext : SpawnPoints")] Transform[] spawnPoints,
        [Inject] ControllersManager<EGameState> controllersManager,
        [Inject(Id = "GameContext : onDeath")] ISubscriptionProperty onDeath,
        [Inject(Id = "GameContext : onResurrection")] ISubscriptionProperty onResurrection,
        [Inject(Id = "GameContext : onDefeat")] ISubscriptionProperty onDefeat,
        [Inject(Id = "GameContext : onRetry")] ISubscriptionProperty onRetry,
        // InputInjector
        [Inject(Id = "InputInjector : onAxisShift")] ISubscriptionProperty<Vector2> onAxisShift,
        [Inject(Id = "InputInjector : OnFire")] ISubscriptionProperty onFireKeyPress,
        [Inject(Id = "InputInjector : onResurrect")] ISubscriptionProperty onResurrectInput,
        // ShootingInjector
        [Inject(Id = "Shooting : onLaunch")] ISubscriptionProperty<ProjectileLaunchData> onLaunch,
        [Inject(Id = "Shooting : OnRemoteHit")] ISubscriptionProperty<ProjectileView> onRemoteHit,
        [Inject(Id = "Shooting : TargetSurvey")] ISubscriptionSurvey<Transform> targetSurvey)
    {
        var resurrectionRegistrator = new ResurrectionViewRegistrator(onResurrection);
        
        var resurrectionGO      = PhotonCore.Instance.InstantiateInstance(resurrectionViewPrefab);
        var resurrectionView    = resurrectionGO.GetComponent<ResurectionView>();
        
        var resurrectionController = new ResurrectionController(resurrectionView);

        var playerService = new PlayerService(resurrectionController, onDefeat, onRetry);

        var character   = PhotonCore.Instance.Character;
        var spawnPoint  = spawnPoints[PhotonCore.Instance.SpawnPointIndex];

        var prefab = Resources.Load<GameObject>(character.Path);

        var go = Object.Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        var view = go.GetComponent<PlayerView>();

        var sentryServiceGO = PhotonCore.Instance.InstantiateInstance(sentryServicePrefab);
        var sentryService   = sentryServiceGO.GetComponent<PlayerSentryService>();

        sentryService
            .NetworkInstantiate(
                character.Path,
                go,
                spawnPoint.position,
                spawnPoint.rotation);

        var playerMoveController = new PlayerMoveController(go.transform, character.Speed, camera);

        onAxisShift.Subscribe(playerMoveController.Move);

        var resurrectionScaner =
            new ContactScaner(
                view.ResurrectionCollider,
                LayerMask.GetMask(Layers.RESURRECTION_TARGET),
                true,
                resurrectionContactsAmount);

        var healthController = new HealthController(character.Health);

        healthController.AddDeathListener(onDeath.Invoke);
        healthController.AddHealthChangedListener(value => onPlayerHealthChanged.Value = value);

        var playerPhysicsController =
            new PlayerPhysicsController(
                view,
                healthController,
                resurrectionScaner,
                onResurrectionContact,
                onCheckResurrectNecessity,
                onResurrectInput,
                onRemoteHit);

        var projectilePoolData      = new ProjectilePoolData(character.ProjectileData, view.Sentry, LayerMask.GetMask(Layers.ENEMY));
        var projectileLaunchData    = new ProjectileLaunchData(projectilePoolData, view.Muzzles);

        var playerGunController     = new PlayerGunController(projectileLaunchData, onLaunch, character.WeaponRechargeTime, onFireKeyPress);

        onResurrectionContact.Subscribe(playerService.Resurrect);
        
        onDeath.Subscribe(view.Kill);

        onResurrection.Subscribe(view.Sentry.Observe);
        onResurrection.Subscribe(healthController.Recover);
        
        targetSurvey.Subscribe(playerService.GetEnemyTarget);

        controllersManager.AddController(resurrectionRegistrator, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause, EGameState.Victory);
        controllersManager.AddController(resurrectionController, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause, EGameState.Victory);
        controllersManager.AddController(playerService, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause);
        controllersManager.AddController(playerPhysicsController, EGameState.Gameplay, EGameState.Pause);
        controllersManager.AddController(playerMoveController, EGameState.Gameplay);
        controllersManager.AddController(playerGunController, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause, EGameState.Victory);

        disposer.Subscribe(playerService.Dispose);
        disposer.Subscribe(playerGunController.Dispose);
        disposer.Subscribe(healthController.Dispose);
    }

    #endregion
}
