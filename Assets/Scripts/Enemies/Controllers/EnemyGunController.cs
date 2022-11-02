using System;

public class EnemyGunController : GunController, IUpdate, IDisposableAdvanced
{
    #region Fields

    private bool _isDisposed;
    private AlarmClock _weaponRechargeClock;

    #endregion

    #region Interface properties

    public bool IsDisposed => _isDisposed;

    #endregion

    #region Constructors

    public EnemyGunController(
        ProjectileLaunchData launchData,
        ISubscriptionProperty<ProjectileLaunchData> onLaunchSubscription,
        float weaponRechargeTime) : base(launchData, onLaunchSubscription)
    {
        _weaponRechargeClock = new AlarmClock(weaponRechargeTime);

        _weaponRechargeClock.OnAlarm += Fire;
        _weaponRechargeClock.OnAlarm += _weaponRechargeClock.Restart;
    }

    #endregion

    #region Interface methods

    public void Dispose()
    {
        if (_isDisposed) return;

        _isDisposed = true;

        _weaponRechargeClock.Dispose();

        GC.SuppressFinalize(this);
    }

    public void OnUpdate(float deltaTime)
    {
        _weaponRechargeClock.OnUpdate(deltaTime);
    }

    #endregion

    #region Methods

    public void Reset()
    {
        _weaponRechargeClock.Restart();
    }

    #endregion
}
