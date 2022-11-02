using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class ShipCoefficients
{
    #region Fields

    [SerializeField] private EEnemyType _type;
    [SerializeField] private float _ship;
    [SerializeField] private float _speed;
    [SerializeField] private float _reload;

    #endregion

    #region Properties

    public EEnemyType Type => _type;
    public float Ship => _ship;
    public float Speed => _speed;
    public float Reload => _reload;

    #endregion
}
