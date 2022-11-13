using UnityEngine;
using Photon.Pun;

public class ProjectileView : PhotonSentryObserved, ISentryObservedCrucial, ISpecialEffectSource
{
    #region Variables

    private bool _isHitRead;

    #endregion

    #region Fields

    [Header("Components")]
    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _launchClip;
    
    [Header("Animations")]
    [SerializeField] private AnimationClip _hitAnimation;

    private int _damage;
    
    private bool _isHit;
    
    private int _hitSentryID;
    private int _hitPhotonViewID;
    private Vector2 _hitPosition;

    #endregion

    #region Observers

    private ISubscriptionProperty<ProjectileView> _onHit;
    private ISubscriptionSurvey<SpecialEffectController> _specialEffectSurvey;
    
    #endregion

    #region Properties

    public int Damage
    {
        get => _damage;
        set => _damage = value;
    }
    public int HitSentryID => _hitSentryID;
    public int HitPhotonViewID => _hitPhotonViewID;
    public Collider2D Collider => _collider;
    public ISubscriptionProperty<ProjectileView> OnHit
    {
        set => _onHit = value;
    }
    public ISubscriptionSurvey<SpecialEffectController> SpecialEffectSurvey
    {
        set => _specialEffectSurvey = value;
    }
    
    #endregion

    #region Unity events

    private void Start()
    {
        PhotonCore.Instance.OnViewInstantiated(this);
    }

    #endregion

    #region Interface methods

    public void OnSentryObserveCrucial(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_isHit);

            if (_isHit)
            {
                stream.SendNext(_hitSentryID);
                stream.SendNext(_hitPhotonViewID);
                stream.SendNext(_hitPosition);
            };
        }
        else
        {
            _isHitRead = (bool) stream.ReceiveNext();

            if (_isHitRead)
            {
                _hitSentryID        = (int) stream.ReceiveNext();
                _hitPhotonViewID    = (int) stream.ReceiveNext();
                _hitPosition        = (Vector2) stream.ReceiveNext();
            };

            if (_isHit != _isHitRead && _isHitRead)
            {
                HitRead(_hitSentryID, _hitPhotonViewID, _hitPosition);
            };
        };
    }

    #endregion

    #region Methods

    public override void Enable()
    {
        _isHit                  = false;
        _collider.enabled       = true;
        _spriteRenderer.enabled = true;

        AudioSource.PlayClipAtPoint(_launchClip, transform.position);
    }

    public override void Disable()
    {
        _collider.enabled       = false;
        _spriteRenderer.enabled = false;
    }
    
    public void Hit(int hitSentryID, int hitPhotonViewID)
    {
        _isHit              = true;
        _hitSentryID        = hitSentryID;
        _hitPhotonViewID    = hitPhotonViewID;

        _onHit.Value = this;
        
        _specialEffectSurvey
            .Get()
            .Play(_hitAnimation, transform.position);

        _hitPosition = transform.position;

        _sentry.Stop();
    }

    private void HitRead(int hitSentryID, int hitPhotonViewID, Vector2 hitPosition)
    {
        _isHit = true;
        _hitSentryID = hitSentryID;
        _hitPhotonViewID = hitPhotonViewID;

        _onHit.Value = this;

        _specialEffectSurvey
            .Get()
            .Play(_hitAnimation, hitPosition);
    }

    #endregion
}
