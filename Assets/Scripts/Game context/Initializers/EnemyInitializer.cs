using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyInitializer
{
    #region Injected methods

    [Inject]
    private void Initialize(
        // AbstractionsInjector
        [Inject] Disposer disposer,
        // GameContextInjector
        [Inject(Id = "GameContext : onRetry")] ISubscriptionProperty onRetry,
        [Inject] ControllersManager<EGameState> controllersManager,
        // ShootingSettings
        [Inject(Id = "ShootingSettings : hitContactsAmount")] int hitContactsAmount,
        // ShootingInjector
        [Inject(Id = "Shooting : onLaunch")] ISubscriptionProperty<ProjectileLaunchData> onLaunch,
        [Inject(Id = "Shooting : OnRemoteHit")] ISubscriptionProperty<ProjectileView> onRemoteHit,
        [Inject(Id = "Shooting : TargetSurvey")] ISubscriptionSurvey<Transform> targetSurvey,
        [Inject(Id = "Shooting : DefaultTarget")] Transform defaultTarget,
        // EnemySettings
        [Inject(Id = "EnemySettings : SpawnerHeatQuantity")] int spawnerHeatQuantity,
        [Inject(Id = "EnemySettings : SpawnerBufferQuantity")] int spawnerBufferQuantity,
        [Inject(Id = "EnemySettings : SpawnInterval")] float spawnTime,
        [Inject(Id = "EnemySettings : SentryService")] string sentryServicePrefab,
        [Inject] EnemySequenceData enemySequence,
        // EnemiesInjector
        [Inject(Id = "EnemiesInjector : Pool")] Transform pool,
        [Inject(Id = "EnemiesInjector : MovementMap")] EnemyMap map,
        [Inject(Id = "EnemiesInjector : OnEnemySequenceEnd")] ISubscriptionProperty onEnemySequenceEnd,
        [Inject(Id = "EnemiesInjector : onAllEnemiesDestroyed")] ISubscriptionProperty onAllEnemiesDestroyed,
        [Inject(Id = "EnemiesInjector : OnAddController")] ISubscriptionProperty<EnemyController> onAddController,
        [Inject(Id = "EnemiesInjector : OnRemoveController")] ISubscriptionProperty<EnemyController> onRemoveController)
    {
        var sentryServiceGO = PhotonCore.Instance.InstantiateInstance(sentryServicePrefab);
        var sentryService   = sentryServiceGO.GetComponent<EnemySentryService>();

        sentryService.Pool  = pool;

        var mapController   = new EnemyMapController(map);

        var enemies = enemySequence.Enemies;

        var spawners = new Dictionary<string, EnemySpawner>();

        for (int i = 0; i < enemies.Length; i++)
        {
            var enemy = enemies[i];

            var enemyPoolData =
                new EnemyPoolData(
                    enemy,
                    pool,
                    hitContactsAmount,
                    disposer,
                    onLaunch);

            var spawner =
                new EnemySpawner(
                    enemyPoolData,
                    sentryService,
                    spawnerBufferQuantity,
                    spawnerHeatQuantity);

            spawners.Add(enemyPoolData.Data.Path, spawner);
        };

        var enemyPacks = new LinkedList<List<string>>();

        for (int p = 0; p < enemySequence.Packs.Length; p++)
        {
            var enemylist = new List<string>();

            var enemyPack = enemySequence.Packs[p];

            for (int e = 0; e < enemyPack.Enemies.Length; e++)
            {
                var enemyQuantity = enemyPack.Enemies[e];

                for (int q = 0; q < enemyQuantity.Quantity; q++)
                {
                    enemylist.Add(enemyQuantity.Data.Path);
                };
            };

            enemyPacks.AddLast(enemylist);
        };

        var enemyService    = new EnemyService(onRemoveController, onRemoteHit);
        var moveService     = new EnemyMoveService(mapController);
        var targetService   = new EnemyTargetService(defaultTarget, targetSurvey);
        var enemyGunService = new EnemyGunService();

        var enemySpawnerService =
            new EnemySpawnerService(
                spawnTime,
                mapController,
                spawners,
                enemyPacks,
                onAddController,
                onEnemySequenceEnd);

        var enemySequenceController = new EnemySequenceController(onAllEnemiesDestroyed);

        onRetry.Subscribe(enemySequenceController.OnRestart);
        onRetry.Subscribe(enemySpawnerService.RestartEnemyPackSequence);
        onRetry.Subscribe(enemyService.Clear);

        onEnemySequenceEnd.Subscribe(enemySequenceController.OnEnemySequenceEnd);

        onAddController.Subscribe(enemyService.OnAddController);
        onAddController.Subscribe(moveService.OnAddController);
        onAddController.Subscribe(targetService.OnAddController);
        onAddController.Subscribe(enemyGunService.OnAddController);
        onAddController.Subscribe(enemySequenceController.OnAddController);

        onRemoveController.Subscribe(enemySpawnerService.OnRemoveController);
        onRemoveController.Subscribe(moveService.OnRemoveController);
        onRemoveController.Subscribe(targetService.OnRemoveController);
        onRemoveController.Subscribe(enemyGunService.OnRemoveController);
        onRemoveController.Subscribe(enemySequenceController.OnRemoveController);

        controllersManager.AddController(enemyService, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause);
        controllersManager.AddController(moveService, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause);
        controllersManager.AddController(targetService, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause);
        controllersManager.AddController(enemyGunService, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause);
        controllersManager.AddController(enemySpawnerService, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause);
        controllersManager.AddController(enemySequenceController, EGameState.Defeat, EGameState.Destroyed, EGameState.Gameplay, EGameState.Pause, EGameState.Quit, EGameState.Victory);
    }

    #endregion
}
