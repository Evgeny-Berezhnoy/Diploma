using UnityEngine;

public class MoveController : IController
{
    #region Fields

    protected float _speed;
    protected Transform _transform;

    #endregion

    #region Properties

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public Transform Transform => _transform;

    #endregion

    #region Constructors

    public MoveController(Transform transform, float speed)
    {
        _transform  = transform;
        _speed      = speed;
    }

    #endregion

    #region Interfaces Methods

    public virtual void Move(Vector2 direction)
    {
        _transform.Translate(direction * _speed, Space.World);
    }

    #endregion
}
