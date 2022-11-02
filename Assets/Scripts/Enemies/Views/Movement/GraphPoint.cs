using System.Collections.Generic;
using UnityEngine;

public class GraphPoint : MonoBehaviour
{
    #region Fields

    [SerializeField] private List<GraphPoint> _connections = new List<GraphPoint>();
    [SerializeField] private Color _gizmosColor = new Color(1, 0, 0);

    #endregion

    #region Properties

    public List<GraphPoint> Connections => _connections;

    #endregion

    #region Unity events

    #if UNITY_EDITOR
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _gizmosColor;

        for (int i = 0; i < _connections.Count; i++)
        {
            if (!_connections[i]) continue;

            Gizmos.DrawLine(transform.position, _connections[i].transform.position);
        };
    }

    #endif

    #endregion
}
