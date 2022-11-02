using System;
using UnityEngine;

[Serializable]
public class PlayfabPlayerStatistic
{
    #region Static fields

    public static readonly EPlayfabPlayerStatisticName RESURRECTIONS = EPlayfabPlayerStatisticName.Resurrections;
    public static readonly EPlayfabPlayerStatisticName KILLS         = EPlayfabPlayerStatisticName.Kills;
    public static readonly EPlayfabPlayerStatisticName POINTS        = EPlayfabPlayerStatisticName.Points;
    
    #endregion

    #region Fields

    [SerializeField] private EPlayfabPlayerStatisticName _name;
    [SerializeField] private int _value;

    #endregion

    #region Properties

    public EPlayfabPlayerStatisticName Name
    {
        get => _name;
        set => _name = value;
    }
    public int Value
    {
        get => _value;
        set => _value = value;
    }

    #endregion

    #region Constructors

    public PlayfabPlayerStatistic(EPlayfabPlayerStatisticName statisticName, int value)
    {
        _name   = statisticName;
        _value  = value;
    }

    #endregion
}
