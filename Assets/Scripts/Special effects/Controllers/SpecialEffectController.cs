using UnityEngine;

public class SpecialEffectController : IUpdate
{
    #region Fields

    private SpecialEffectView _view;
    
    private bool _followRoot;
    private Transform _root;

    #endregion

    #region Properties

    public SpecialEffectView View => _view;
    public bool NeedsToDispose => !_view.IsPlaying;

    #endregion

    #region Constructors

    public SpecialEffectController(SpecialEffectView view)
    {
        _view = view;
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        if (!_followRoot) return;

        _view.transform.position = _root.position;
    }

    #endregion

    #region Methods

    public void Play(AnimationClip animation, Transform transform)
    {
        _followRoot = true;

        _root = transform;

        _view.transform.position = _root.position;

        _view.Play(animation);
    }

    public void Play(AnimationClip animation, Vector2 position)
    {
        _followRoot = false;

        _view.transform.position = position;

        _view.Play(animation);
    }

    #endregion
}
