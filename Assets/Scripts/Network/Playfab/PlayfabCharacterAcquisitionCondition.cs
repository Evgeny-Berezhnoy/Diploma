using System;
using UnityEngine;

[Serializable]
public class PlayfabCharacterAcquisitionCondition
{
    #region Fields

    [SerializeField] public EPlayfabPlayerStatisticName _statistic;
    [SerializeField] public int _value;

    #endregion

    #region Properties

    public EPlayfabPlayerStatisticName Statistic => _statistic;
    public int Value => _value;

    #endregion
}
