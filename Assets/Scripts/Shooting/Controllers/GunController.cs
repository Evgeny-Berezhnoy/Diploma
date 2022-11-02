public class GunController : IController
{
    #region Fields

    protected ProjectileLaunchData _launchData;

    #endregion

    #region Observers

    protected ISubscriptionProperty<ProjectileLaunchData> _onLaunchSubscription;

    #endregion

    #region Constructors

    public GunController(
        ProjectileLaunchData launchData,
        ISubscriptionProperty<ProjectileLaunchData> onLaunchSubscription)
    {
        _launchData = launchData;

        _onLaunchSubscription = onLaunchSubscription;
    }

    #endregion

    #region Methods

    protected virtual void Fire()
    {
        _onLaunchSubscription.Value = _launchData;
    }

    #endregion
}
