using System.Collections.Generic;
using UnityEngine;

public class EnemyMap : MonoBehaviour
{
    #region Fields

    [SerializeField] private List<GraphPoint> _spawnPoints;
    [SerializeField] private List<GraphPoint> _despawnPoints;
    
    #endregion

    #region Properties

    public List<GraphPoint> SpawnPoints => _spawnPoints;
    public List<GraphPoint> DespawnPoints => _despawnPoints;

    #endregion
}
