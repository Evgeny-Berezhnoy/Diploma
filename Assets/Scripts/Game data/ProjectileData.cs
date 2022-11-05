using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Game/Data/Projectile")]
public class ProjectileData : ScriptableObject, IGameData, IPoolData
{
    #region Fields

    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifeTime;
    [SerializeField] private string _path;

    #endregion

    #region Interface properties

    public string Path => _path;

    #endregion

    #region Properties

    public int Damage => _damage;
    public float Speed => _speed;
    public float LifeTime => _lifeTime;

    #endregion
}
