using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonSentry))]
public class EnemyView : PhotonSentryObserved, ISentryObservedCrucial, ISpecialEffectSource
{
    #region Variables

    private bool _isKilledRead;

    #endregion

    #region Fields

    [Header("Transforms")]
    [SerializeField] private Transform[] _muzzles;
    
    [Header("Components")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _hitCollider;

    [Header("Animations")]
    [SerializeField] private AnimationClip _deathClip;
    
    private bool _isKilled;
    private Vector2 _killingPosition;

    #endregion

    #region Observers

    private ISubscriptionSurvey<SpecialEffectController> _specialEffectSurvey;

    #endregion

    #region Properties

    public Transform[] Muzzles => _muzzles;
    public Collider2D HitCollider => _hitCollider;
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
            stream.SendNext(_isKilled);

            if (_isKilled)
            {
                stream.SendNext(_killingPosition);
            };
        }
        else
        {
            _isKilledRead = (bool) stream.ReceiveNext();

            if (_isKilledRead)
            {
                _killingPosition = (Vector2) stream.ReceiveNext();
            };

            if (_isKilled != _isKilledRead && _isKilledRead)
            {
                KillRead(_killingPosition);
            };
        };
    }

    #endregion

    #region Methods

    public override void Enable()
    {
        _isKilled               = false;
        _spriteRenderer.enabled = true;
        _hitCollider.enabled    = true;
    }

    public override void Disable()
    {
        _spriteRenderer.enabled = false;
        _hitCollider.enabled    = false;
    }
    
    public void Kill()
    {
        _isKilled = true;

        _specialEffectSurvey
            .Get()
            .Play(_deathClip, transform.position);

        _killingPosition = transform.position;

        _sentry.Stop();
    }

    private void KillRead(Vector2 position)
    {
        _isKilled = true;

        _specialEffectSurvey
            .Get()
            .Play(_deathClip, position);
    }

    #endregion
}
