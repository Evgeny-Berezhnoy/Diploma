using Photon.Pun;

public class ProjectileSentryService : PhotonSentryService
{
    #region Variables

    private int _damage;
    private int _poolSentryID;
    private int _poolPhotonViewID;

    #endregion

    #region Fields

    private object[] PARAMETERS = new object[3];
    
    #endregion

    #region Base methods

    protected override void NetworkInitialize(PhotonSentry sentry, object[] customData)
    {
        _damage             = (int) customData[0];
        _poolSentryID       = (int) customData[1];
        _poolPhotonViewID   = (int) customData[2];

        for(_index = 0; _index < sentry.Components.Length; _index++)
        {
            _observedComponent = sentry.Components[_index];

            if(_observedComponent is ProjectileView projectileView)
            {
                projectileView.Damage = _damage;
            }
            else if(_observedComponent is PhotonSentryTransformObserved transformObserved)
            {
                transformObserved.SetPool(_poolSentryID, _poolPhotonViewID, true);
            };
        };

        sentry.Stop();
    }

    [PunRPC]
    protected override void NetworkInstantiate_RPC(string prefab, object[] customData)
    {
        base.NetworkInstantiate_RPC(prefab, customData);
    }

    #endregion

    #region Methods

    public void NetworkInstantiate(
        string prefab,
        int damage,
        int poolSentryID,
        int poolPhotonViewID)
    {
        PARAMETERS[0] = damage;
        PARAMETERS[1] = poolSentryID;
        PARAMETERS[2] = poolPhotonViewID;

        base.NetworkInstantiate(prefab, PARAMETERS);
    }

    #endregion
}
