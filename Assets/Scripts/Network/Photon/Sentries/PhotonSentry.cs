using System;
using UnityEngine;
using Photon.Pun;

public class PhotonSentry : MonoBehaviour, IPunObservable
{
    #region Variables

    private bool _isObservingRead;
    private int _index;

    #endregion

    #region Fields

    private ISentryObservedComponent[] _components;
    private ISentryObservedCrucial[] _observedCrucial;
    private ISentryObserved[] _observed;
    private PhotonView _photonView;
    private int _id;
    private bool _isObserving;

    #endregion

    #region Properties

    public ISentryObservedComponent[] Components => _components;
    public PhotonView PhotonView
    {
        get => _photonView;
        set => _photonView = value;
    }
    public int PhotonViewID
    {
        get => _photonView.ViewID;
    }
    public int ID
    {
        get => _id;
        set => _id = value;
    }
    public bool IsObserving => _isObserving;

    #endregion

    #region Unity events

    private void Awake()
    {
        _isObserving = true;

        _components         = GetComponents<ISentryObservedComponent>();
        _observedCrucial    = GetComponents<ISentryObservedCrucial>();
        _observed           = GetComponents<ISentryObserved>();

        for(_index = 0; _index < _components.Length; _index++)
        {
            _components[_index].Sentry = this;
        };
    }

    #endregion

    #region Interface methods

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_isObserving);

            for (_index = 0; _index < _observedCrucial.Length; _index++)
            {
                _observedCrucial[_index].OnSentryObserveCrucial(stream, info);
            };

            if (_isObserving)
            {
                for(_index = 0; _index < _observed.Length; _index++)
                {
                    _observed[_index].OnSentryObserve(stream, info);
                };
            };
        }
        else
        {
            _isObservingRead = (bool) stream.ReceiveNext();

            if (_isObserving != _isObservingRead && _isObservingRead)
            {
                Observe();
            }
            else if (_isObserving != _isObservingRead && !_isObservingRead)
            {
                Stop();
            };

            for (_index = 0; _index < _observedCrucial.Length; _index++)
            {
                _observedCrucial[_index].OnSentryObserveCrucial(stream, info);
            };

            if (_isObserving)
            {
                for (_index = 0; _index < _observed.Length; _index++)
                {
                    _observed[_index].OnSentryObserve(stream, info);
                };
            };
        };
    }

    #endregion

    #region Methods

    public void Observe()
    {
        if (_isObserving) return;

        _isObserving = true;

        for(_index = 0; _index < _components.Length; _index++)
        {
            _components[_index].Enable();
        };
    }

    public void Stop()
    {
        if (!_isObserving) return;
        
        _isObserving = false;

        for (_index = 0; _index < _components.Length; _index++)
        {
            _components[_index].Disable();
        };
    }
    
    #endregion
}
