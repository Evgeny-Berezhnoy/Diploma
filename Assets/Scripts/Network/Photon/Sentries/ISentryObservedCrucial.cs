using Photon.Pun;

public interface ISentryObservedCrucial
{
    #region Methods

    void OnSentryObserveCrucial(PhotonStream stream, PhotonMessageInfo info);

    #endregion
}
