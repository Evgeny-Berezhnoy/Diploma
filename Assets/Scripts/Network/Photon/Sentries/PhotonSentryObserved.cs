using UnityEngine;

public abstract class PhotonSentryObserved : MonoBehaviour, ISentryObservedComponent
{
    #region Fields

    protected PhotonSentry _sentry;

    #endregion

    #region Properties

    public PhotonSentry Sentry
    {
        get => _sentry;
        set => _sentry = value;
    }

    #endregion

    #region Interface methods

    public abstract void Disable();
    public abstract void Enable();
    
    #endregion
}
