public interface ISentryObservedComponent
{
    #region Properties

    PhotonSentry Sentry { get; set; }

    #endregion

    #region Methods

    void Enable();
    void Disable();

    #endregion
}
