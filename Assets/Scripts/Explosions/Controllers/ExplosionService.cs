using System.Collections.Generic;
using UnityEngine;

public class ExplosionService : IUpdate
{
    #region Fields

    private ExplosionSpawner _spawner;
    private List<ExplosionController> _controllers;

    private ExplosionController _controller;

    #endregion

    #region Controllers

    public ExplosionService(
        ExplosionSpawner spawner,
        ISubscriptionProperty<Transform> onExplosion)
    {
        _spawner = spawner;

        _controllers = new List<ExplosionController>();
        
        onExplosion.Subscribe(OnExplosion);
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        for(int i = _controllers.Count - 1; i >= 0; i--)
        {
            _controller = _controllers[i];
            _controller.OnUpdate(deltaTime);

            if (_controller.NeedsToDispose)
            {
                _controllers.Remove(_controller);

                _spawner.Push(_controller);
            };
        };
    }

    #endregion

    #region Methods

    private void OnExplosion(Transform pool)
    {
        _controller = _spawner.Pop();

        _controller.View.transform.position = pool.transform.position;

        _controllers.Add(_controller);
    }

    #endregion
}
