using Photon.Pun;

public class ProjectilePhysicsController : IFixedUpdate
{
    #region Fields

    private ProjectileView _view;
    private ContactScaner _hitScaner;
    
    #endregion

    #region Properties

    public ProjectileView View => _view;
    public ContactScaner HitScaner => _hitScaner;

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
        if (_view.NeedsToDispose) return;

        if (_hitScaner.Collider.enabled)
        {
            _hitScaner.OnFixedUpdate(fixedDeltaTime);

            for(int i = 0; i < _hitScaner.ContactsAmount; i++)
            {
                var collider = _hitScaner.Contacts[i];

                var photonViewID =
                    collider
                        .transform
                        .GetComponentInParent<PhotonView>()
                        .ViewID;

                _view.Hit(photonViewID);
            };
        };
    }

    #endregion
}
