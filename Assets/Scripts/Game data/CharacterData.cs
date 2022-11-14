using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Game/Data/Character")]
public class CharacterData : ScriptableObject, IGameData, IShip
{
    #region Fields

    [SerializeField] private string _name;
    [SerializeField] private string _path;
    [SerializeField] private string _dataPath;
    [SerializeField, Range(1, 10)] private int _health;
    [SerializeField, Range(3f, 20f)] private float _speed;
    [SerializeField, Range(0.05f, 5f)] private float _weaponRechargeTime;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private ProjectileData _projectileData;

    #endregion

    #region Interface properties

    public string Path => _path;
    public int Health => _health;
    public float Speed => _speed;
    public float WeaponRechargeTime => _weaponRechargeTime;

    #endregion

    #region Properties

    public string Name => _name;
    public string DataPath => _dataPath;
    public Sprite Sprite => _sprite;
    public ProjectileData ProjectileData => _projectileData;

    #endregion
}
