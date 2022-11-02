using System.Collections.Generic;
using UnityEngine;

public class EnemyMapController : IController
{
    #region Fields

    private EnemyMap _map;
    private Dictionary<Transform, GraphPoint> _occupiedPoints;

    #endregion

    #region Properties

    public bool HasFreeSpawnPoints
    {
        get
        {
            for(int i = 0; i < _map.SpawnPoints.Count; i++)
            {
                if(HasFreeDestinations(_map.SpawnPoints[i], out var _))
                {
                    return true;
                };
            };

            return false;
        }
    }

    #endregion

    #region Constructors

    public EnemyMapController(EnemyMap map)
    {
        _map            = map;
        _occupiedPoints = new Dictionary<Transform, GraphPoint>();
    }

    #endregion

    #region Methods

    public bool TryOccupyNewPoint(Transform enemy, out Transform newDestination, out bool endOfItinarary)
    {
        newDestination = null;

        endOfItinarary = false;

        if (_occupiedPoints.TryGetValue(enemy, out var graphPoint))
        {
            if(_map.DespawnPoints.Contains(graphPoint))
            {
                DeoccupyPoint(enemy);

                endOfItinarary = true;

                return false;
            };

            if(!HasFreeDestinations(graphPoint, out var freeDestinations))
            {
                return false;
            };
            
            _occupiedPoints[enemy] = freeDestinations.Random();

            newDestination = _occupiedPoints[enemy].transform;

            return true;
        }
        else
        {
            var freeSpawnPoints = new Dictionary<GraphPoint, List<GraphPoint>>();

            for(int i = 0; i < _map.SpawnPoints.Count; i++)
            {
                var enterPoint = _map.SpawnPoints[i];

                if(!HasFreeDestinations(enterPoint, out var destinationPoints))
                {
                    continue;
                };

                freeSpawnPoints.Add(enterPoint, destinationPoints);
            };

            if (freeSpawnPoints.Count == 0)
            {
                return false;
            };

            var enemyEnterPointData = freeSpawnPoints.Random();

            enemy
                .SetPositionAndRotation(
                    enemyEnterPointData.Key.transform.position,
                    enemyEnterPointData.Key.transform.rotation);

            var enemyDestination = enemyEnterPointData.Value.Random();

            _occupiedPoints.Add(enemy, enemyDestination);

            newDestination = enemyDestination.transform;

            return true;
        };
    }

    public void DeoccupyPoint(Transform enemy)
    {
        if (!_occupiedPoints.ContainsKey(enemy)) return;

        _occupiedPoints.Remove(enemy);
    }

    private bool HasFreeDestinations(GraphPoint currentPoint, out List<GraphPoint> freeDestinations)
    {
        freeDestinations = new List<GraphPoint>();

        for (int i = 0; i < currentPoint.Connections.Count; i++)
        {
            var destination = currentPoint.Connections[i];

            if (!_occupiedPoints.ContainsValue(destination))
            {
                freeDestinations.Add(destination);
            };
        };

        return (freeDestinations.Count > 0);
    }

    #endregion
}
