using System;
using UnityEngine;

[Serializable]
public class EnemyPack
{
    #region Fields

    [SerializeField] private EnemyQuantity[] _enemies;

    #endregion

    #region Properties

    public EnemyQuantity[] Enemies => _enemies;

    #endregion
}
