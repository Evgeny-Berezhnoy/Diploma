using UnityEngine;

public class ProjectileMoveController : MoveController, IUpdate
{
    #region Fields

    private float _lifeTime;
    private float _currentLifeTime;
    private bool _isAlive;

    #endregion

    #region Properties

    public bool IsAlive => _isAlive;

    #endregion

    #region Constructors

    public ProjectileMoveController(Transform transform, float speed, float lifeTime) : base(transform, speed)
    {
        _lifeTime = lifeTime;

        Reset();
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        _currentLifeTime += deltaTime;
        _currentLifeTime = Mathf.Clamp(_currentLifeTime, 0, _lifeTime);

        _isAlive = (_currentLifeTime < _lifeTime) && _transform;

        if (_isAlive)
        {
            Move(_transform.up * deltaTime);
        };
    }

    #endregion

    #region Methods

    public void Reset()
    {
        _currentLifeTime    = 0;
        _isAlive            = true;
    }

    #endregion
}
