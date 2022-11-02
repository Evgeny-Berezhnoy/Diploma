using UnityEngine;

public class PlayerPhysicsController : IFixedUpdate
{
    #region Fields

    private PlayerView _view;
    private HealthController _healthController;
    private ContactScaner _resurrectionScaner;
    
    private Collider2D _resurrectionCollider;

    #endregion

    #region Observers

    private ISubscriptionProperty<Collider2D> _onResurrectionContact;

    #endregion

    #region Constructors

    public PlayerPhysicsController(
        PlayerView view,
        HealthController healthController,
        ContactScaner resurrectionScaner,
        ISubscriptionProperty<Collider2D> onResurrectionContact,
        ISubscriptionProperty onResurrectInput,
        ISubscriptionMessenger<int, HealthController> onRemoteHit)
    {
        _view               = view;
        _healthController   = healthController;
        _resurrectionScaner = resurrectionScaner;
        
        _onResurrectionContact  = onResurrectionContact;

        onResurrectInput.Subscribe(CheckResurrection);
        onRemoteHit.Subscribe(OnRemoteHit);
    }

    #endregion

    #region Interface methods

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        _resurrectionScaner.OnFixedUpdate(fixedDeltaTime);
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
