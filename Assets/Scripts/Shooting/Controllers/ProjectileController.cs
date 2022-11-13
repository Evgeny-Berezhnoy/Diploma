public class ProjectileController : IController
{
    #region Fields
    
    private ProjectileView _view;
    private PhotonSentry _pool;
    private ProjectileMoveController _moveController;
    private ProjectilePhysicsController _physicsController;

    #endregion

    #region Properties

    public ProjectileView View => _view;
    public PhotonSentry Pool => _pool;
    public ProjectileMoveController MoveController => _moveController;
    public ProjectilePhysicsController PhysicsController => _physicsController;
    public bool NeedsToDispose => (!_view.Sentry.IsObserving || !_moveController.IsAlive);

    #endregion

    #region Constructors

    public ProjectileController(
        ProjectileView view,
        PhotonSentry pool,
        ProjectileMoveController moveController,
        ProjectilePhysicsController physicsController)
    {
        _view               = view;
        _pool               = pool;
        _moveController     = moveController;
        _physicsController  = physicsController;
    }

    #endregion

    #region Methods

    public void Reset()
    {
        _moveController.Reset();
    }

    #endregion
}
