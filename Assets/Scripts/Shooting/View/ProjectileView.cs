using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class ProjectileView : MonoBehaviour, IPunObservable
{
    #region Fields

    [SerializeField] private Collider2D _collider;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _launchClip;
    [SerializeField] private AudioClip _hitClip;

    private PhotonView _photonView;
    
    private int _poolViewID;
    private Transform _pool;

    private int _hitViewID;
    private int _damage;

    private bool _isHit;
    private bool _isLaunched;
    private bool _needsToDispose;

    #endregion

    #region Observers

    private ISubscriptionMessenger<int, HealthController> _onHit;

    #endregion

    #region Properties

    public int Damage
    {
        get => _damage;
        set => _damage = value;
    }
    public int Pool
    {
        set
        {
            if (_poolViewID == value) return;

            _poolViewID = value;
            _pool       = PhotonView.Find(_poolViewID).transform;

            if (!_isLaunched)
            {
                transform.SetParent(_pool);
                transform.SetPositionAndRotation(_pool.position, _pool.rotation);
            };
        }
    }
    public bool NeedsToDispose => _needsToDispose;
    public Collider2D Collider => _collider;
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
    public ISubscriptionMessenger<int, HealthController> OnHit
    {
        set => _onHit = value;
    }

    #endregion

    #region Unity events

    private void Start()
    {
        if (!_isLaunched)
        {
            _collider.enabled       = false;
            _spriteRenderer.enabled = false;
        };

        PhotonCore.Instance.OnViewInstantiated(this.PhotonView);
    }

    #endregion

    #region Interface methods

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_damage);
            stream.SendNext(_isLaunched);
            stream.SendNext(_isHit);
            stream.SendNext(_hitViewID);
            stream.SendNext(_poolViewID);
        }
        else
        {
            _damage         = (int)  stream.ReceiveNext();
            
            var isLaunched  = (bool) stream.ReceiveNext();
            var isHit       = (bool) stream.ReceiveNext();
            var hitViewID   = (int)  stream.ReceiveNext();
            var pool        = (int)  stream.ReceiveNext();

            Pool = pool;

            if (_isHit != isHit && isHit)
            {
                Hit(hitViewID);
            };

            if (_isLaunched != isLaunched && isLaunched)
            {
                Launch();
            }
            else if(_isLaunched != isLaunched && !isLaunched)
            {
                Hide();
            };
        };
    }

    #endregion

    #region Methods

    public void Launch()
    {
        _isHit                  = false;
        _isLaunched             = true;
        _needsToDispose         = false;
        _collider.enabled       = true;
        _spriteRenderer.enabled = true;

        transform.SetParent(null);
        
        AudioSource.PlayClipAtPoint(_launchClip, transform.position);
    }

    public void Hide()
    {
        _isLaunched             = false;
        _needsToDispose         = true;
        _collider.enabled       = false;
        _spriteRenderer.enabled = false;

        transform.SetParent(_pool);
        transform.SetPositionAndRotation(_pool.position, _pool.rotation);
    }
    
    public void Hit(int hitViewID)
    {
        _isHit          = true;
        _hitViewID      = hitViewID;

        _onHit.Value    = hitViewID;
        
        if(_onHit.Result != _onHit.DefaultResult)
        {
            _onHit.Result.Hurt(_damage);
        };

        Hide();
        
        AudioSource.PlayClipAtPoint(_hitClip, transform.position);
    }

    #endregion
}
