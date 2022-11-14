using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Game/Data/Enemy")]
public class EnemyData : ScriptableObject, IGameData, IPoolData, IShip
{
    #region Fields

    [SerializeField] private string _path;
    [SerializeField, Range(1, 10)] private int _health;
    [SerializeField, Range(0.5f, 10f)] private float _speed;
    [SerializeField, Range(0.5f, 10f)] private float _weaponRechargeTime;
    [SerializeField, Range(0.5f, 30f)] private float _overwatchTime;
    [SerializeField, Range(10f, 720f)] private float _targetRotationSpeed;
    [SerializeField] private ProjectileData _projectileData;

    #endregion

    #region Interface properties

    public string Path => _path;
    public int Health => _health;
    public float Speed => _speed;
    public float WeaponRechargeTime => _weaponRechargeTime;

    #endregion

    #region Properties

    public float OverwatchTime => _overwatchTime;
    public float TargetRotationSpeed => _targetRotationSpeed;
    public ProjectileData ProjectileData => _projectileData;

    #endregion
}
