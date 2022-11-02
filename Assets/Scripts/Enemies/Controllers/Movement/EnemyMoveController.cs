using UnityEngine;

public class EnemyMoveController : MoveController, IUpdate
{
    #region Fields

    private Transform _target;
    private EEnemyMovementPhase _currentPhase;
    private float _overwatchTime;
    private float _overwatchTimeCurrent;

    #endregion

    #region Properties

    public Transform Target
    {
        get => _target;
        set
        {
            _target = value;

            ChangePhase(EEnemyMovementPhase.TargetMovement);
        }
    }

    public EEnemyMovementPhase CurrentPhase
    {
        get => _currentPhase;
        set => ChangePhase(value);
    }

    #endregion

    #region Constructors

    public EnemyMoveController(Transform transform, float speed, float overwatchTime) : base(transform, speed)
    {
        _overwatchTime = overwatchTime;
    }

    #endregion

    #region Base methods

    public override void Move(Vector2 direction)
    {
        _transform.Translate(direction, Space.World);
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        if(_currentPhase == EEnemyMovementPhase.TargetMovement)
        {
            TargetMovement(deltaTime);
        }
        else if (_currentPhase == EEnemyMovementPhase.Overwatch)
        {
            Overwatch(deltaTime);
        };
    }

    private void TargetMovement(float deltaTime)
    {
        var movementVector      = (Vector2)(_target.position - _transform.position).normalized * deltaTime * _speed;
        var destinationVector   = (Vector2)(_target.position - _transform.position);

        if(movementVector.sqrMagnitude > destinationVector.sqrMagnitude)
        {
            movementVector = destinationVector;

            ChangePhase(EEnemyMovementPhase.Overwatch);
        };

        Move(movementVector);
    }

    private void Overwatch(float deltaTime)
    {
        _overwatchTimeCurrent += deltaTime;
        _overwatchTimeCurrent = Mathf.Clamp(_overwatchTimeCurrent, 0, _overwatchTime);

        if(_overwatchTimeCurrent == _overwatchTime)
        {
            ChangePhase(EEnemyMovementPhase.AwaitingDestination);
        };
    }

    private void ChangePhase(EEnemyMovementPhase phase)
    {
        _currentPhase = phase;

        _overwatchTimeCurrent = 0;
    }

    #endregion
}
