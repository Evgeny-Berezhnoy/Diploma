using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Player statistics", menuName = "PlayFab/Custom/Player statistics")]
public class PlayfabPlayerStatistics : ScriptableObject
{
    #region Fields

    [SerializeField] private PlayfabPlayerStatistic[] _statistics;

    #endregion

    #region Properties

    public PlayfabPlayerStatistic[] Statistics
    {
        get => _statistics;
        set => _statistics = value;
    }

    #endregion

    #region Static methods

    public static PlayfabPlayerStatistic[] Empty()
    {
        return new PlayfabPlayerStatistic[]
        {
            new PlayfabPlayerStatistic(PlayfabPlayerStatistic.RESURRECTIONS, 0),
            new PlayfabPlayerStatistic(PlayfabPlayerStatistic.KILLS, 0),
            new PlayfabPlayerStatistic(PlayfabPlayerStatistic.POINTS, 0)
        };
    }

    public static List<EPlayfabPlayerStatisticName> List()
    {
        return new List<EPlayfabPlayerStatisticName>()
        {
            PlayfabPlayerStatistic.RESURRECTIONS,
            PlayfabPlayerStatistic.KILLS,
            PlayfabPlayerStatistic.POINTS
        };
    }

    #endregion
}
