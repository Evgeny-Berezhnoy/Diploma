using UnityEngine;

public sealed class EnemyPoolData : NetworkPoolData<EnemyData, Transform>
{
    #region Fields

    private int _projectileHitContactsAmount;
    private Disposer _disposer;

    #endregion

    #region Observers

    private ISubscriptionProperty<ProjectileLaunchData> _onLaunchSubscription;
    
    #endregion

    #region Properties

    public int ProjectileHitContactsAmount => _projectileHitContactsAmount;
    public Disposer Disposer => _disposer;
    public ISubscriptionProperty<ProjectileLaunchData> OnLaunchSubscription => _onLaunchSubscription;
    
    #endregion

    #region Constructors

    public EnemyPoolData(
        EnemyData data,
        Transform pool,
        int projectileHitContactsAmount,
        Disposer disposer,
        ISubscriptionProperty<ProjectileLaunchData> onLaunchSubscription)
    :
    base(
        data,
        pool)
    {
        _projectileHitContactsAmount    = projectileHitContactsAmount;
        _disposer                       = disposer;

        _onLaunchSubscription = onLaunchSubscription;
    }

    #endregion
}
