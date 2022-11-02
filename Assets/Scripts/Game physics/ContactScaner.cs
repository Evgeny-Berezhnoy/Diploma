using UnityEngine;

public class ContactScaner : IFixedUpdate
{
    #region Fields

    private Collider2D _collider;
    private ContactFilter2D _filter;
    private Collider2D[] _contacts;
    private Collider2D[] _rawContacts;
    private int _contactsAmount;

    #endregion

    #region Properties

    public Collider2D Collider => _collider;
    public int ContactsAmount => _contactsAmount;
    public Collider2D[] Contacts => _contacts;

    #endregion

    #region Constructors

    public ContactScaner(
        Collider2D collider,
        LayerMask layerMask,
        bool useTriggers = true,
        int contactsNumber = 1)
    {
        _collider = collider;

        _filter = new ContactFilter2D();
        _filter.SetLayerMask(layerMask);
        _filter.useTriggers = useTriggers;

        _contacts       = new Collider2D[contactsNumber];
        _rawContacts    = new Collider2D[contactsNumber];
        
        _contactsAmount = 0;
    }

    #endregion

    #region Interface methods

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        for(int i = 0; i < _rawContacts.Length; i++)
        {
            _rawContacts[i] = null;
        };

        for (int i = 0; i < _contactsAmount; i++)
        {
            _contacts[i] = null;
        };

        _contactsAmount = 0;

        if (!_collider.enabled) return;

        _collider.OverlapCollider(_filter, _rawContacts);

        for(int i = 0; i < _rawContacts.Length; i++)
        {
            if (_rawContacts[i] == null) continue;

            _contacts[_contactsAmount] = _rawContacts[i];

            _contactsAmount += 1;
        };
    }

    #endregion
}
