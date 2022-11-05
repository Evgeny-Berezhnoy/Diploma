using UnityEngine;
using Photon.Pun;

public class EnemyView : MonoBehaviour, IPunObservable, IExplosive
{
    #region Fields

    [Header("Transforms")]
    [SerializeField] private Transform[] _muzzles;
    [SerializeField] private PhotonView _projectilePool;

    [Header("Components")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _hitCollider;
    
    private int _poolViewID;
    private Transform _pool;

    private bool _isDead;
    private bool _isKilled;

    private PhotonView _photonView;

    #endregion

    #region Observers

    private ISubscriptionProperty<Transform> _onExplosion;

    #endregion

    #region Properties

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
    public PhotonView ProjectilePool => _projectilePool;
    public Collider2D HitCollider => _hitCollider;
    public bool IsDead => _isDead;
    public int Pool
    {
        set
        {
            if (_poolViewID == value) return;

            _poolViewID = value;
            _pool       = PhotonView.Find(_poolViewID).transform;

            if (_isDead)
            {
                PlaceInPool();
            };
        }
    }
    public ISubscriptionProperty<Transform> OnExplosion
    {
        set => _onExplosion = value;
    }

    #endregion

    #region Unity events

    private void Start()
    {
        _hitCollider.enabled    = !_isDead;
        _spriteRenderer.enabled = !_isDead;

        PhotonCore.Instance.OnViewInstantiated(PhotonView);
    }

    #endregion

    #region Interface methods

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_isDead);
            stream.SendNext(_isKilled);
            stream.SendNext(_poolViewID);
        }
        else
        {
            var isDead      = (bool) stream.ReceiveNext();
            var isKilled    = (bool) stream.ReceiveNext();
            var pool        = (int)  stream.ReceiveNext();

            Pool = pool;

            if (_isKilled != isKilled && isKilled)
            {
                Kill();
            };

            if (_isDead != isDead && !isDead)
            {
                Spawn();
            }
            else if (_isDead != isDead && isDead)
            {
                Despawn();
            };
        };
    }

    #endregion

    #region Methods

    public void Spawn()
    {
        _isKilled               = false;
        _isDead                 = false;
        _spriteRenderer.enabled = true;
        _hitCollider.enabled    = true;

        transform.SetParent(null);
    }

    public void Kill()
    {
        _isKilled = true;

        _onExplosion.Value = transform;

        Despawn();
    }
    
    public void Despawn()
    {
        _isDead                 = true;
        _spriteRenderer.enabled = false;
        _hitCollider.enabled    = false;

        PlaceInPool();
    }
    
    private void PlaceInPool()
    {
        transform.SetParent(_pool);
        transform.SetPositionAndRotation(_pool.position, _pool.rotation);
    }

    #endregion
}
