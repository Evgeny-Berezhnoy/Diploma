using UnityEngine;

public class ProjectileSpawner : NetworkControllerSpawner<ProjectileController, ProjectileView, ProjectileSentryService>
{
    #region Variables

    private PhotonSentryTransformObserved _transformObserved;
    private ProjectileMoveController _moveController;
    private ContactScaner _hitScaner;
    private ProjectilePhysicsController _physicsController;

    #endregion

    #region Fields

    private int _hitContactsAmount;
    private ProjectilePoolData _poolData;

    #endregion

    #region Properties

    public PhotonSentry Pool => _poolData.Pool;

    #endregion

    #region Constructors

    public ProjectileSpawner(
        ProjectilePoolData poolData,
        ProjectileSentryService sentryService,
        int bufferQuantity,
        int hitContactsAmount)
    :
    base(
        poolData.Data.Path,
        sentryService,
        bufferQuantity)
    {
        _poolData           = poolData;
        _hitContactsAmount  = hitContactsAmount;
    }

    public ProjectileSpawner(
        ProjectilePoolData poolData,
        ProjectileSentryService sentryService,
        int bufferQuantity,
        int hitContactsAmount,
        int heatQuantity)
    :
    this(
        poolData,
        sentryService,
        bufferQuantity,
        hitContactsAmount)
    {
        Heat(heatQuantity);
    }

    #endregion

    #region Base methods

    public override ProjectileController Pop()
    {
        var controller = base.Pop();

        controller.MoveController.Reset();

        return controller;
    }

    protected override ProjectileView GetView(ProjectileController controller)
    {
        return controller.View;
    }

    protected override void EnableView(ProjectileView view)
    {
        view.Sentry.Observe();
    }

    protected override void DisableView(ProjectileView view)
    {
        view.Sentry.Stop();
    }

    protected override void NetworkInstantiate()
    {
        _sentryService
            .NetworkInstantiate(
                _templatePath,
                _poolData.Data.Damage,
                _poolData.Pool.ID,
                _poolData.Pool.PhotonViewID);
    }

    protected override ProjectileController CreateController(GameObject go)
    {
        _view = go.GetComponent<ProjectileView>();
        
        _view.Damage = _poolData.Data.Damage;

        _moveController =
            new ProjectileMoveController(
                _view.transform,
                _poolData.Data.Speed,
                _poolData.Data.LifeTime);

        _hitScaner =
            new ContactScaner(
                _view.Collider,
                _poolData.LayerMask,
                true,
                _hitContactsAmount);

        _physicsController =
            new ProjectilePhysicsController(
                _view,
                _hitScaner);

        _controller =
            new ProjectileController(
                _view,
                _poolData.Pool,
                _moveController,
                _physicsController);

        _transformObserved = go.GetComponent<PhotonSentryTransformObserved>();
        _transformObserved.SetPool(_poolData.Pool.ID, _poolData.Pool.PhotonViewID, true);
        
        return _controller;
    }

    #endregion
}
