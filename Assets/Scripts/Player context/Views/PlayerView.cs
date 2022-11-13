using Photon.Pun;
using UnityEngine;

public class PlayerView : PhotonSentryObserved, ISentryObservedCrucial, ISpecialEffectSource
{
    #region Variables

    private bool _isKilledRead;

    #endregion

    #region Fields

    [Header("Transforms")]
    [SerializeField] private Transform _enemyTarget;
    [SerializeField] private Transform _wholeSprites;
    [SerializeField] private Transform _debrisSprites;
    [SerializeField] private Transform[] _muzzles;
    
    [Header("Components")]
    [SerializeField] private Collider2D _hitCollider;
    [SerializeField] private Collider2D _resurrectionCollider;
    [SerializeField] private Collider2D _resurrectionTargetCollider;

    [Header("Animations")]
    [SerializeField] private AnimationClip _deathClip;
    [SerializeField] private AnimationClip _resurrectionClip;

    private bool _isKilled;
    private Vector2 _killingPosition;

    #endregion

    #region Observers

    private ISubscriptionSurvey<SpecialEffectController> _specialEffectSurvey;

    #endregion

    #region Properties

    public Transform EnemyTarget => _enemyTarget;
    public Transform[] Muzzles => _muzzles;
    public Collider2D ResurrectionCollider => _resurrectionCollider;
    public Collider2D ResurrectionTargetCollider => _resurrectionTargetCollider;
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

            if(_isKilled != _isKilledRead && _isKilledRead)
            {
                KillRead(_killingPosition);
            };
        };
    }

    public override void Enable()
    {
        _isKilled = false;

        _specialEffectSurvey
            .Get()
            .Play(_resurrectionClip, transform);

        SetProperties();
    }

    public override void Disable()
    {
        SetProperties();
    }

    #endregion

    #region Methods

    public void Kill()
    {
        _isKilled = true;

        _specialEffectSurvey
            .Get()
            .Play(_deathClip, transform);

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

    private void SetProperties()
    {
        _enemyTarget.gameObject.SetActive(_sentry.IsObserving);
        _wholeSprites.gameObject.SetActive(_sentry.IsObserving);
        _debrisSprites.gameObject.SetActive(!_sentry.IsObserving);

        _hitCollider.enabled                = _sentry.IsObserving;
        _resurrectionCollider.enabled       = _sentry.IsObserving;
        _resurrectionTargetCollider.enabled = !_sentry.IsObserving;
    }

    #endregion
}
