using UnityEngine;
using Photon.Pun;

public class ExplosiveViewRegistrator : IController
{
    #region Fields

    private IExplosive _view;

    #endregion

    #region Observers

    private ISubscriptionProperty<Transform> _onExplosion;

    #endregion

    #region Constructors

    public ExplosiveViewRegistrator(ISubscriptionProperty<Transform> onExplosion)
    {
        _onExplosion = onExplosion;

        PhotonCore.Instance.AddViewInstantiationListener(OnViewInstantiated);
    }

    #endregion

    #region Methods

    private void OnViewInstantiated(PhotonView photonView)
    {
        _view =
            photonView
                .gameObject
                .GetComponent<IExplosive>();

        if (_view == null) return;

        _view.OnExplosion = _onExplosion;
    }

    #endregion
}
