using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy sequence", menuName = "Game/Enemy sequence")]
public class EnemySequenceData : ScriptableObject
{
    #region Fields

    [SerializeField] private EnemyPack[] _packs;

    #endregion

    #region Properties

    public EnemyPack[] Packs => _packs;
    public EnemyData[] Enemies
    {
        get
        {
            return
                _packs
                    .GroupBy(x => x.Enemies)
                    .SelectMany(x => x.Key)
                    .GroupBy(x => x.Data)
                    .Select(x => x.Key)
                    .ToArray();
        }
    }

    #endregion
}
