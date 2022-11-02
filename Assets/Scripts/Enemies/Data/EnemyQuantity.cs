using System;
using UnityEngine;

[Serializable]
public class EnemyQuantity
{
    #region Fields

    [SerializeField] private EnemyData _data;
    [SerializeField, Range(1, 8)] private int _quantity;

    #endregion

    #region Properties

    public EnemyData Data => _data;
    public int Quantity => _quantity;

    #endregion
}
