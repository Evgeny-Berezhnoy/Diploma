using UnityEngine;
using Photon.Pun;

public class PhotonSentryTransformObserved : PhotonTransformView, ISentryObservedComponent, ISentryObserved
{
    #region Variables

    private int _index;
    private PhotonSentry _sentryFound;
    private PhotonSentry[] _sentries;

    #endregion

    #region Fields

    private PhotonSentry _sentry;
    private Transform _pool;
    
    #endregion

    #region Interface properties

    public PhotonSentry Sentry
    {
        get => _sentry;
        set => _sentry = value;
    }

    #endregion

    #region Interface methods

    public void Enable()
    {
        m_SynchronizePosition = true;
        m_SynchronizeRotation = true;
        
        transform.SetParent(null);
    }

    public void Disable()
    {
        m_SynchronizePosition = false;
        m_SynchronizeRotation = false;

        if (_pool == null) return;

        m_NetworkPosition   = _pool.position;
        m_StoredPosition    = _pool.position;

        Hide();
    }

    public void OnSentryObserve(PhotonStream stream, PhotonMessageInfo info)
    {
        OnPhotonSerializeView(stream, info);
    }
    
    #endregion

    #region Methods

    public void SetPool(int sentryID, int photonViewID = 0, bool hide = false)
    {
        _pool = null;

        if (photonViewID == 0) return;

        _sentries = FindObjectsOfType<PhotonSentry>();

        for (_index = 0; _index < _sentries.Length; _index++)
        {
            _sentryFound = _sentries[_index];

            if (_sentryFound.ID == sentryID && _sentryFound.PhotonViewID == photonViewID)
            {
                _pool = _sentryFound.transform;

                if (hide) Hide(); 

                break;
            };
        };
    }

    public void SetPool(Transform pool, bool hide = false)
    {
        _pool = pool;

        if (hide) Hide();
    }

    private void Hide()
    {
        transform.SetParent(_pool);

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    #endregion
}
