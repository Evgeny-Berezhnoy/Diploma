using UnityEngine;
using Photon.Pun;

public class EnemySentryService : PhotonSentryService
{
    #region Fields

    private Transform _pool;
    private object[] PARAMETERS = new object[0];

    #endregion

    #region Properties

    public Transform Pool
    {
        set => _pool = value;
    }

    #endregion

    #region Unity events

    protected override void Awake()
    {
        base.Awake();

        if (_photonView.IsMine) return;

        _pool = FindObjectOfType<PoolView>().transform;
    }

    #endregion

    #region Base methods

    protected override void NetworkInitialize(PhotonSentry sentry, params object[] customData)
    {
        for (_index = 0; _index < sentry.Components.Length; _index++)
        {
            _observedComponent = sentry.Components[_index];

            if (_observedComponent is PhotonSentryTransformObserved transformObserved)
            {
                transformObserved.SetPool(_pool, true);
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

    public void NetworkInstantiate(string prefab)
    {
        base.NetworkInstantiate(prefab, PARAMETERS);
    }

    #endregion
}
