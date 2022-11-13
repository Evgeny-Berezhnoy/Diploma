public class EnemyController : IController
{
    #region Fields

    private string _template;
    private EnemyView _view;
    private EnemyMoveController _moveController;
    private EnemyTargetController _targetController;
    private EnemyGunController _gunController;
    private HealthController _healthController;

    #endregion

    #region Properties

    public string Template => _template;
    public EnemyView View => _view;
    public EnemyMoveController MoveController => _moveController;
    public EnemyTargetController TargetController => _targetController;
    public EnemyGunController GunController => _gunController;
    public HealthController HealthController => _healthController;
    public bool NeedsToDispose => !_view.Sentry.IsObserving || (_moveController.CurrentPhase == EEnemyMovementPhase.Disabled);

    #endregion

    #region Constructors

    public EnemyController(
        string template,
        EnemyView view,
        EnemyMoveController moveController,
        EnemyTargetController targetcontroller,
        EnemyGunController gunController,
        HealthController healthController)
    {
        _template           = template;
        _view               = view;
        _moveController     = moveController;
        _targetController   = targetcontroller;
        _gunController      = gunController;
        _healthController   = healthController;
    }

    #endregion
}
