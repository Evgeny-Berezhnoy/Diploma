using System.Collections.Generic;
using System.Linq;
using Photon.Pun;

public class ProjectileService : IUpdate, IFixedUpdate
{
    #region Fields

    private int _spawnerHeatQuantity;
    private int _spawnerBufferQuantity;
    private int _hitContactsAmount;
    private List<ProjectileSpawner> _spawners;
    private List<ProjectileController> _controllers;
    
    #endregion

    #region Constructors

    public ProjectileService(
        int spawnerHeatQuantity,
        int spawnerBufferQuantity,
        int hitContactsAmount,
        ISubscriptionProperty<ProjectileLaunchData> onLaunch)
    {
        _spawnerHeatQuantity    = spawnerHeatQuantity;
        _spawnerBufferQuantity  = spawnerBufferQuantity;
        _hitContactsAmount      = hitContactsAmount;
        _spawners               = new List<ProjectileSpawner>();
        _controllers            = new List<ProjectileController>();

        onLaunch.Subscribe(Launch);
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        for(int i = _controllers.Count - 1; i >= 0; i--)
        {
            var controller = _controllers[i];

            controller.OnUpdate(deltaTime);

            if (controller.NeedsToDispose)
            {
                _controllers.RemoveAt(i);

                var spawner = GetProjectileSpawner(controller.Pool);

                spawner.Push(controller);
            };
        };
    }

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        for (int i = _controllers.Count - 1; i >= 0; i--)
        {
            var controller = _controllers[i];

            controller.OnFixedUpdate(fixedDeltaTime);

            if (controller.NeedsToDispose)
            {
                _controllers.RemoveAt(i);

                var spawner = GetProjectileSpawner(controller.Pool);

                spawner.Push(controller);
            };
        };
    }

    #endregion

    #region Methods

    private void Launch(ProjectileLaunchData launchData)
    {
        var spawner = GetProjectileSpawner(launchData.PoolData.Pool);

        if (spawner == default(ProjectileSpawner))
        {
            spawner =
                new ProjectileSpawner(
                    launchData.PoolData,
                    _hitContactsAmount,
                    _spawnerBufferQuantity,
                    _spawnerHeatQuantity);

            _spawners.Add(spawner);
        };

        for(int i = 0; i < launchData.SpawnPoints.Length; i++)
        {
            var spawnPoint = launchData.SpawnPoints[i];

            var controller = spawner.Pop();

            controller
                .View
                .transform
                .SetPositionAndRotation(
                    spawnPoint.position,
                    spawnPoint.rotation);

            _controllers.Add(controller);
        };
    }

    private ProjectileSpawner GetProjectileSpawner(PhotonView pool)
    {
        return _spawners.FirstOrDefault(x => x.Root == pool);
    }

    #endregion
}
