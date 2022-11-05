using UnityEngine;

[CreateAssetMenu(fileName = "Explosion", menuName = "Game/Data/Explosion")]
public class ExplosionData : ScriptableObject, IPoolData
{
    #region Fields

    [SerializeField] private GameObject _prefab;
    [SerializeField, Range(0.05f, 0.5f)] private float _spriteTransitionTime;

    #endregion

    #region Interface properties

    public GameObject Prefab => _prefab;
    public float SpriteTransitionTime => _spriteTransitionTime;
    
    #endregion
}
