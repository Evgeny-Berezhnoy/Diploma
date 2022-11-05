using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class PlayerView : MonoBehaviour, IPunObservable, IExplosive
{
    #region Fields

    [Header("Transforms")]
    [SerializeField] private Transform _enemyTarget;
    [SerializeField] private Transform _wholeSprites;
    [SerializeField] private Transform _debrisSprites;
    [SerializeField] private Transform[] _muzzles;
    [SerializeField] private PhotonView _projectilePool;
    
    [Header("Components")]
    [SerializeField] private Collider2D _hitCollider;
    [SerializeField] private Collider2D _resurrectionCollider;
    [SerializeField] private Collider2D _resurrectionTargetCollider;
    [SerializeField] private AudioClip _resurrectionClip;

    private bool _isDead;

    private PhotonView _photonView;

    #endregion

    #region Observers

    private ISubscriptionProperty<Transform> _onExplosion;

    #endregion

    #region Properties

    public Transform EnemyTarget => _enemyTarget;
    public Transform[] Muzzles => _muzzles;
    public PhotonView PhotonView
    {
        get
        {
            if (!_photonView)
            {
                _photonView = PhotonView.Get(this);
            };

            return _photonView;
        }
    }
    public Collider2D HitCollider => _hitCollider;
    public Collider2D ResurrectionCollider => _resurrectionCollider;
    public Collider2D ResurrectionTargetCollider => _resurrectionTargetCollider;
    public PhotonView ProjectilePool => _projectilePool;
    public bool IsDead => _isDead;
    public ISubscriptionProperty<Transform> OnExplosion
    {
        set => _onExplosion = value;
    }

    #endregion

    #region Unity events

    private void Start()
    {
        PhotonCore.Instance.OnViewInstantiated(this.PhotonView);
    }

    #endregion

    #region Interface methods
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_isDead);
        }
        else
        {
            var isDead = (bool) stream.ReceiveNext();

            if(_isDead != isDead && !isDead)
            {
                Resurrect();
            }
            else if(_isDead != isDead && isDead)
            {
                Die();
            };
        };
    }
    
    #endregion

    #region Methods

    public void Resurrect()
    {
        if (!_isDead) return;
    
        _isDead = false;

        SetProperties();

        AudioSource.PlayClipAtPoint(_resurrectionClip, transform.position);
    }

    public void Die()
    {
        if (_isDead) return;

        _isDead = true;

        _onExplosion.Value = transform;

        SetProperties();
    }

    private void SetProperties()
    {
        _enemyTarget.gameObject.SetActive(!_isDead);
        _wholeSprites.gameObject.SetActive(!_isDead);
        _debrisSprites.gameObject.SetActive(_isDead);

        _hitCollider.enabled                = !_isDead;
        _resurrectionCollider.enabled       = !_isDead;
        _resurrectionTargetCollider.enabled = _isDead;
    }

    #endregion
}
