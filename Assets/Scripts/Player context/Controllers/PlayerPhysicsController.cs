using UnityEngine;

public class PlayerPhysicsController : IFixedUpdate
{
    #region Fields

    private PlayerView _view;
    private HealthController _healthController;
    private ContactScaner _resurrectionScaner;
    
    private Collider2D _resurrectionCollider;

    private bool _resurrectionIsNeeded;
    private bool _resurrectionIsNeededCache;

    #endregion

    #region Observers

    private ISubscriptionProperty<Collider2D> _onResurrectionContact;
    private ISubscriptionProperty<bool> _onCheckResurrectNecessity;

    #endregion

    #region Constructors

    public PlayerPhysicsController(
        PlayerView view,
        HealthController healthController,
        ContactScaner resurrectionScaner,
        ISubscriptionProperty<Collider2D> onResurrectionContact,
        ISubscriptionProperty<bool> onCheckResurrectNecessity,
        ISubscriptionProperty onResurrectInput,
        ISubscriptionMessenger<int, HealthController> onRemoteHit)
    {
        _view               = view;
        _healthController   = healthController;
        _resurrectionScaner = resurrectionScaner;
        
        _onResurrectionContact      = onResurrectionContact;
        _onCheckResurrectNecessity  = onCheckResurrectNecessity;

        onResurrectInput.Subscribe(CheckResurrection);
        onRemoteHit.Subscribe(OnRemoteHit);
    }

    #endregion

    #region Interface methods

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        _resurrectionScaner.OnFixedUpdate(fixedDeltaTime);

        _resurrectionIsNeeded = (_resurrectionScaner.ContactsAmount > 0);

        if (_resurrectionIsNeeded == _resurrectionIsNeededCache) return;

        _resurrectionIsNeededCache = _resurrectionIsNeeded;

        _onCheckResurrectNecessity.Value = _resurrectionIsNeeded;
    }

    #endregion

    #region Methods

    private void CheckResurrection()
    {
        if (!_resurrectionScaner.Collider.enabled) return;
            
        for (int i = 0; i < _resurrectionScaner.ContactsAmount; i++)
        {
            _resurrectionCollider = _resurrectionScaner.Contacts[i];

            _onResurrectionContact.Value = _resurrectionCollider;
        };
    }

    private HealthController OnRemoteHit(int photonViewID)
    {
        if(_view.PhotonView.ViewID == photonViewID)
        {
            return _healthController;
        }

        return null;
    }

    #endregion
}
