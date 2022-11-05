using UnityEngine;

public class ExplosionController : IUpdate
{
    #region Fields

    private float _spriteTransitionTime;
    private float _spriteTransitionTimeCurrent;

    private ExplosionView _view;
    
    private bool _needsToDispose;

    #endregion

    #region Properties

    public ExplosionView View => _view;
    public bool NeedsToDispose => _needsToDispose;

    #endregion

    #region Constructors

    public ExplosionController(ExplosionView view, float spriteTransitionTime)
    {
        _spriteTransitionTime = spriteTransitionTime;

        _view = view;
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        _spriteTransitionTimeCurrent += deltaTime;
        _spriteTransitionTimeCurrent = Mathf.Clamp(_spriteTransitionTimeCurrent, 0, _spriteTransitionTime);

        if(_spriteTransitionTimeCurrent < _spriteTransitionTime) return;

        if(_view.SpriteNumber < _view.SpritesMaxIndex)
        {
            _view.SpriteNumber++;

            _spriteTransitionTimeCurrent = 0;
        }
        else
        {
            _needsToDispose = true;
        };          
    }

    #endregion

    #region Methods

    public void Reset()
    {
        _needsToDispose = false;

        _spriteTransitionTimeCurrent = 0;
    }

    #endregion
}
