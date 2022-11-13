using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class PhotonSentryService : MonoBehaviour
{
    #region Variables

    protected int _index;
    protected GameObject _go;
    protected PhotonSentry _sentry;
    protected ISentryObservedComponent _observedComponent;

    #endregion

    #region Fields

    private int _sentryID;
    protected PhotonView _photonView;
    protected List<GameObject> _observedGameobjects;

    #endregion

    #region Properties

    public PhotonView PhotonView => _photonView;

    #endregion

    #region Unity events

    protected virtual void Awake()
    {
        _sentryID = -1;

        _photonView = PhotonView.Get(this);

        _observedGameobjects = new List<GameObject>();
    }

    protected virtual void OnDestroy()
    {
        for(_index = 0; _index < _observedGameobjects.Count; _index++)
        {
            Destroy(_observedGameobjects[_index]);
        };
    }

    #endregion

    #region Methods

    protected void NetworkInstantiate(string path, object[] customData)
    {
        _photonView.RPC(nameof(NetworkInstantiate_RPC), RpcTarget.Others, path, customData);
    }
    
    protected virtual void NetworkInstantiate_RPC(string prefab, object[] customData)
    {
        _go = Instantiate(Resources.Load<GameObject>(prefab));

        AddSentry(_go);

        NetworkInitialize(_sentry, customData);
    }

    public PhotonSentry AddSentry(GameObject go)
    {
        _observedGameobjects.Add(go);

        _sentry = go.GetComponent<PhotonSentry>();

        _sentry.PhotonView  = _photonView;
        _sentry.ID          = NewSentryID();

        for (_index = 0; _index < _sentry.Components.Length; _index++)
        {
            _observedComponent = _sentry.Components[_index];

            if(_observedComponent is PhotonSentryTransformObserved transformObserved)
            {
                transformObserved.photonView = _photonView;
            };
        };

        _photonView.ObservedComponents.Add(_sentry);

        return _sentry;
    }

    protected virtual int NewSentryID()
    {
        _sentryID++;

        return _sentryID;
    }

    protected abstract void NetworkInitialize(PhotonSentry sentry, object[] customData);

    #endregion
}
