using UnityEngine;

public class EnemySpawner : NetworkControllerSpawner<EnemyController, EnemyView, EnemySentryService>
{
    #region Variables

    private PhotonSentryTransformObserved _transformObserved;
    private EnemyMoveController _moveController;
    private EnemyTargetController _targetController;
    private ProjectilePoolData _projectilePoolData;
    private ProjectileLaunchData _projectileLaunchData;
    private EnemyGunController _gunController;
    private HealthController _healthController;

    #endregion

    #region Fields

    private EnemyPoolData _poolData;

    #endregion

    #region Constructors

    public EnemySpawner(
        EnemyPoolData poolData,
        EnemySentryService sentryService,
        int bufferQuantity)
    :
    base(
        poolData.Data.Path,
        sentryService,
        bufferQuantity)
    {
        _poolData = poolData;
    }

    public EnemySpawner(
        EnemyPoolData poolData,
        EnemySentryService sentryService,
        int bufferQuantity,
        int heatQuantity)
    :
    this(
        poolData,
        sentryService,
        bufferQuantity)
    {
        Heat(heatQuantity);
    }

    #endregion

    #region Base methods

    public override EnemyController Pop()
    {
        _controller = base.Pop();

        _controller.GunController.Reset();
        _controller.HealthController.Recover();

        return _controller;
    }

    protected override EnemyView GetView(EnemyController controller)
    {
        return controller.View;
    }

    protected override void EnableView(EnemyView view)
    {
        view.Sentry.Observe();
    }

    protected override void DisableView(EnemyView view)
    {
        view.Sentry.Stop();
    }

    protected override EnemyController CreateController(GameObject go)
    {
        _view = go.GetComponent<EnemyView>();

        _moveController         = new EnemyMoveController(_view.transform, _poolData.Data.Speed, _poolData.Data.OverwatchTime);
        _targetController       = new EnemyTargetController(_poolData.Data.TargetRotationSpeed, _view.transform);

        _projectilePoolData     = new ProjectilePoolData(_poolData.Data.ProjectileData, _view.Sentry, LayerMask.GetMask(Layers.PLAYER));
        _projectileLaunchData   = new ProjectileLaunchData(_projectilePoolData, _view.Muzzles);

        _gunController          = new EnemyGunController(_projectileLaunchData, _poolData.OnLaunchSubscription, _poolData.Data.WeaponRechargeTime);

        _healthController       = new HealthController(_poolData.Data.Health);
        
        _controller =
            new EnemyController(
                _templatePath,
                _view,
                _moveController,
                _targetController,
                _gunController,
                _healthController);

        _transformObserved = go.GetComponent<PhotonSentryTransformObserved>();
        _transformObserved.SetPool(_poolData.Pool, true);
        
        _healthController.AddDeathListener(_view.Kill);
        
        _poolData.Disposer.Subscribe(_gunController.Dispose);
        _poolData.Disposer.Subscribe(_healthController.Dispose);
        
        return _controller;
    }

    protected override void NetworkInstantiate()
    {
        _sentryService.NetworkInstantiate(_templatePath);
    }

    #endregion
}
