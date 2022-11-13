using Photon.Pun;

public interface ISentryObserved
{
    #region Methods

    void OnSentryObserve(PhotonStream stream, PhotonMessageInfo info);

    #endregion
}
