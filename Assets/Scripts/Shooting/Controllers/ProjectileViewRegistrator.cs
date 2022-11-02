using Photon.Pun;

public class ProjectileViewRegistrator : IController
{
    #region Observers

    private ISubscriptionMessenger<int, HealthController> _onHit;

    #endregion

    #region Constructors

    public ProjectileViewRegistrator(ISubscriptionMessenger<int, HealthController> onHit)
    {
        _onHit = onHit;

        PhotonCore.Instance.AddViewInstantiationListener(OnViewInstantiated);
    }

    #endregion

    #region Methods

    private void OnViewInstantiated(PhotonView photonView)
    {
        var view =
            photonView
                .gameObject
                .GetComponent<ProjectileView>();

        if (!view) return;

        view.OnHit = _onHit;
    }

    #endregion
}
