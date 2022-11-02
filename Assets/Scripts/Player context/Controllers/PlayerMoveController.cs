using UnityEngine;

public class PlayerMoveController : MoveController
{
    #region Fields

    private Camera _camera;
    
    #endregion

    #region Constructors

    public PlayerMoveController(Transform transform, float speed, Camera camera) : base(transform, speed)
    {
        _camera = camera;
    }

    #endregion

    #region Base Methods

    public override void Move(Vector2 direction)
    {
        var restrictionX        = _camera.orthographicSize * _camera.aspect;
        var restrictionY        = _camera.orthographicSize;
        var moveDirectionX      = _transform.position.x + direction.x * _speed;
        var moveDirectionY      = _transform.position.y + direction.y * _speed;
        var travelerPosition    = _transform.position;

        if (Mathf.Abs(moveDirectionX) < restrictionX)
        {
            travelerPosition.x = moveDirectionX;
        };

        if (Mathf.Abs(moveDirectionY) < restrictionY)
        {
            travelerPosition.y = moveDirectionY;
        };

        _transform.position = travelerPosition;
    }

    #endregion
}