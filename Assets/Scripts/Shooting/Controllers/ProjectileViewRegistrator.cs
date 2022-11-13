using UnityEngine;

public class ProjectileViewRegistrator : IController
{
    #region Fields

    private ProjectileView _projectile;

    #endregion

    #region Observers

    private ISubscriptionProperty<ProjectileView> _onHit;

    #endregion

    #region Constructors

    public ProjectileViewRegistrator(ISubscriptionProperty<ProjectileView> onHit)
    {
        _onHit = onHit;

        PhotonCore.Instance.AddViewInstantiationListener(OnViewInstantiated);
    }

    #endregion

    #region Methods

    private void OnViewInstantiated(MonoBehaviour view)
    {
        _projectile = view as ProjectileView;

        if (!_projectile) return;

        _projectile.OnHit = _onHit;
    }

    #endregion
}
