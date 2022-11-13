using System.Collections.Generic;

public class EnemyService : IUpdate
{
    #region Variables

    private int _index;
    private EnemyController _controller;

    #endregion

    #region Fields

    private List<EnemyController> _controllers;

    #endregion

    #region Observers

    private ISubscriptionProperty<EnemyController> _onRemoveController;

    #endregion

    #region Constructors

    public EnemyService(
        ISubscriptionProperty<EnemyController> onRemoveController,
        ISubscriptionProperty<ProjectileView> onRemoteHit)
    {
        _controllers        = new List<EnemyController>();

        _onRemoveController = onRemoveController;

        onRemoteHit.Subscribe(GetHealthController);
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        for(_index = _controllers.Count - 1; _index >= 0; _index--)
        {
            _controller = _controllers[_index];

            CheckDespawn(_controller, _index);
        };
    }

    #endregion

    #region Methods

    public void OnAddController(EnemyController controller)
    {
        _controllers.Add(controller);
    }

    public void Clear()
    {
        for (_index = _controllers.Count - 1; _index >= 0; _index--)
        {
            _controller = _controllers[_index];

            _controllers.RemoveAt(_index);

            _onRemoveController.Value = _controller;
        };
    }

    private void CheckDespawn(EnemyController controller, int index)
    {
        if (controller.NeedsToDispose)
        {
            _controllers.RemoveAt(index);

            _onRemoveController.Value = controller;
        };
    }

    private void GetHealthController(ProjectileView projectile)
    {
        for(_index = 0; _index < _controllers.Count; _index++)
        {
            _controller = _controllers[_index];

            if(_controller.View.Sentry.ID == projectile.HitSentryID &&
                _controller.View.Sentry.PhotonViewID == projectile.HitPhotonViewID)
            {
                _controller.HealthController.Hurt(projectile.Damage);

                break;
            };
        };
    }

    #endregion
}
