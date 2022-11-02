using Photon.Pun;

public class ProjectileController : IUpdate, IFixedUpdate
{
    #region Fields
    
    private ProjectileView _view;
    private PhotonView _pool;
    private ProjectileMoveController _moveController;
    private ProjectilePhysicsController _physicsController;

    #endregion

    #region Properties

    public ProjectileView View => _view;
    public PhotonView Pool => _pool;
    public bool NeedsToDispose => (_view.NeedsToDispose || !_moveController.IsAlive);

    #endregion

    #region Constructors

    public ProjectileController(
        ProjectileView view,
        PhotonView pool,
        ProjectileMoveController moveController,
        ProjectilePhysicsController physicsController)
    {
        _view               = view;
        _pool               = pool;
        _moveController     = moveController;
        _physicsController  = physicsController;
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        _moveController.OnUpdate(deltaTime);
    }

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        _physicsController.OnFixedUpdate(fixedDeltaTime);
    }

    #endregion

    #region Methods

    public void Reset()
    {
        _moveController.Reset();
    }

    #endregion
}
