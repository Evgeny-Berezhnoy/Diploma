using UnityEngine;

public class EnemyTargetController : IUpdate
{
    #region Fields

    private float _rotationSpeed;
    private Transform _transform;
    private Transform _target;

    private Vector2 _direction;
    private Quaternion _rotationGoal;

    #endregion

    #region Properties

    public Transform Target
    {
        set => _target = value;
    }
    public bool TargetIsActive => _target && _target.gameObject.activeSelf;

    #endregion

    #region Constructors

    public EnemyTargetController(
        float rotationSpeed,
        Transform transform)
    {
        _rotationSpeed = rotationSpeed;

        _transform = transform;
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        if (!TargetIsActive) return;

        _direction          = _target.position - _transform.position;
        _rotationGoal       = Quaternion.LookRotation(Vector3.forward, _direction);
        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, _rotationGoal, _rotationSpeed * deltaTime);
    }

    #endregion
}
