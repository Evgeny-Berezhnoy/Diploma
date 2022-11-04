using UnityEngine;

public class BackgroundController : IUpdate
{
    #region Fields

    private BackgroundView _view;
    private Camera _camera;
    private float _speed;

    private float _cameraWidth;
    private float _widthRatio;
    private Vector3 _scale;

    private Vector2 _offset;

    #endregion

    #region Constructors

    public BackgroundController(
        BackgroundView view,
        Camera camera,
        float speed)
    {
        _view = view;
        _camera = camera;
        _speed = speed;

        _offset = Vector2.zero;

        AdjustBackgroundToCamera();
    }

    #endregion

    #region Interface properties

    public void OnUpdate(float deltaTime)
    {
        AdjustBackgroundToCamera();

        _offset.y += _speed * deltaTime;
        
        _view.RendererMaterial.mainTextureOffset = _offset;
    }

    #endregion

    #region Methods

    private void AdjustBackgroundToCamera()
    {
        _cameraWidth    = 2 * _camera.orthographicSize * _camera.aspect;
        _widthRatio     = _cameraWidth / _view.Sprite.bounds.size.x;
        _scale          = Vector3.one * _widthRatio;

        _view.transform.localScale = _scale;
    }

    #endregion
}
