using System;

public class PlayerGunController : GunController, IUpdate, IDisposableAdvanced
{
    #region Fields

    private bool _isDisposed;
    private AlarmClock _weaponRechargeClock;

    #endregion

    #region Interface properties

    public bool IsDisposed => _isDisposed;
    
    #endregion

    #region Constructors

    public PlayerGunController(
        ProjectileLaunchData launchData,
        ISubscriptionProperty<ProjectileLaunchData> onLaunchSubscription,
        float weaponRechargeTime,
        ISubscriptionProperty onFire) : base(launchData, onLaunchSubscription)
    {
        _weaponRechargeClock = new AlarmClock(weaponRechargeTime);

        onFire.Subscribe(Fire);
    }

    #endregion

    #region Base methods

    protected override void Fire()
    {
        if (_weaponRechargeClock.IsRunning) return;

        _weaponRechargeClock.Restart();

        base.Fire();
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        _weaponRechargeClock.OnUpdate(deltaTime);
    }

    public void Dispose()
    {
        if (_isDisposed) return;

        _isDisposed = true;

        _weaponRechargeClock.Dispose();

        GC.SuppressFinalize(this);
    }

    #endregion
}
