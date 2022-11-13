using UnityEngine;

public class ProjectilePhysicsController : IFixedUpdate
{
    #region Variables

    private int _index;
    private Collider2D _hitCollider; 
    private PhotonSentry _sentry;

    #endregion

    #region Fields

    private ProjectileView _view;
    private ContactScaner _hitScaner;
    
    #endregion

    #region Constructors

    public ProjectilePhysicsController(ProjectileView view, ContactScaner hitScaner)
    {
        _view       = view;
        _hitScaner  = hitScaner;
    }

    #endregion

    #region Interface methods

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        if (!_view.Sentry.IsObserving) return;

        if (_hitScaner.Collider.enabled)
        {
            _hitScaner.OnFixedUpdate(fixedDeltaTime);

            for(_index = 0; _index < _hitScaner.ContactsAmount; _index++)
            {
                _hitCollider = _hitScaner.Contacts[_index];

                _sentry =
                    _hitCollider
                        .transform
                        .GetComponentInParent<PhotonSentry>();

                _view.Hit(_sentry.ID, _sentry.PhotonViewID);
            };
        };
    }

    #endregion
}
