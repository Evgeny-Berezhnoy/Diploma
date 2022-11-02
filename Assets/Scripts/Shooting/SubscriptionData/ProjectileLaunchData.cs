using UnityEngine;

public sealed class ProjectileLaunchData
{
    #region Fields

    public readonly ProjectilePoolData PoolData;
    public readonly Transform[] SpawnPoints;

    #endregion

    #region Constructors

    public ProjectileLaunchData(ProjectilePoolData poolData, Transform[] spawnPoints)
    {
        PoolData    = poolData; 
        SpawnPoints = spawnPoints;
    }

    #endregion
}
