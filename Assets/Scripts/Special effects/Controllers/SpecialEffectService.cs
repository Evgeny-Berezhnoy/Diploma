using System.Collections.Generic;

public class SpecialEffectService : IUpdate
{
    #region Fields

    private SpecialEffectSpawner _spawner;
    private List<SpecialEffectController> _controllers;

    private int _index;
    private SpecialEffectController _controller;

    #endregion

    #region Controllers

    public SpecialEffectService(
        SpecialEffectSpawner spawner,
        ISubscriptionSurvey<SpecialEffectController> specialEffectSurvey)
    {
        _spawner = spawner;

        _controllers = new List<SpecialEffectController>();

        specialEffectSurvey.Subscribe(OnSurvey);
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        for(_index = _controllers.Count - 1; _index >= 0; _index--)
        {
            _controller = _controllers[_index];
            _controller.OnUpdate(deltaTime);

            if (_controller.NeedsToDispose)
            {
                _controllers.Remove(_controller);

                _spawner.Push(_controller);
            };
        };
    }

    #endregion

    #region Methods

    private SpecialEffectController OnSurvey()
    {
        _controller = _spawner.Pop();

        _controllers.Add(_controller);

        return _controller;
    }

    #endregion
}
