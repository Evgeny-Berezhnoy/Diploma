using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

public class ProjectileService : IFixedUpdate
{
    #region Fields

    private int _spawnerHeatQuantity;
    private int _spawnerBufferQuantity;
    private int _hitContactsAmount;
    private List<ProjectileSpawner> _spawners;
    private List<ProjectileController> _controllers;

    private ProjectileSpawner _spawner;
    private ProjectileController _controller;
    private Transform _spawnPoint;
    private int _index;

    #endregion

    #region Observers

    private ISubscriptionProperty<ProjectileController> _onAddController;
    private ISubscriptionProperty<ProjectileController> _onRemoveController;

    #endregion

    #region Constructors

    public ProjectileService(
        int spawnerHeatQuantity,
        int spawnerBufferQuantity,
        int hitContactsAmount,
        ISubscriptionProperty<ProjectileController> onAddController,
        ISubscriptionProperty<ProjectileController> onRemoveController,
        ISubscriptionProperty<ProjectileLaunchData> onLaunch)
    {
        _spawnerHeatQuantity    = spawnerHeatQuantity;
        _spawnerBufferQuantity  = spawnerBufferQuantity;
        _hitContactsAmount      = hitContactsAmount;
        _spawners               = new List<ProjectileSpawner>();
        _controllers            = new List<ProjectileController>();

        _onAddController    = onAddController;
        _onRemoveController = onRemoveController;

        onLaunch.Subscribe(Launch);
    }

    #endregion

    #region Interface methods

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        for (_index = _controllers.Count - 1; _index >= 0; _index--)
        {
            _controller = _controllers[_index];

            if (_controller.NeedsToDispose)
            {
                RemoveController(_controllers[_index]);
            };
        };
    }

    #endregion

    #region Methods

    private void Launch(ProjectileLaunchData launchData)
    {
        _spawner = GetProjectileSpawner(launchData.PoolData.Pool);

        if (_spawner == default(ProjectileSpawner))
        {
            _spawner =
                new ProjectileSpawner(
                    launchData.PoolData,
                    _hitContactsAmount,
                    _spawnerBufferQuantity,
                    _spawnerHeatQuantity);

            _spawners.Add(_spawner);
        };

        for(_index = 0; _index < launchData.SpawnPoints.Length; _index++)
        {
            _spawnPoint = launchData.SpawnPoints[_index];

            _controller = _spawner.Pop();

            _controller
                .View
                .transform
                .SetPositionAndRotation(
                    _spawnPoint.position,
                    _spawnPoint.rotation);

            _controllers.Add(_controller);

            _onAddController.Value = _controller;
        };
    }

    public void Clear()
    {
        for (_index = _controllers.Count - 1; _index >= 0; _index--)
        {
            RemoveController(_controllers[_index]);
        };
    }
    
    private ProjectileSpawner GetProjectileSpawner(PhotonView pool)
    {
        return _spawners.FirstOrDefault(x => x.Root == pool);
    }

    private void RemoveController(ProjectileController controller)
    {
        _controllers.Remove(controller);

        _onRemoveController.Value = _controller;

        _spawner = GetProjectileSpawner(_controller.Pool);

        _spawner.Push(_controller);
    }

    #endregion
}
