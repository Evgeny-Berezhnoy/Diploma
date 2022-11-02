using UnityEngine;

public class InputAxisController : IUpdate
{
    #region Observers

    private ISubscriptionProperty<Vector2> _onAxisShift;

    #endregion

    #region Constructors

    public InputAxisController(ISubscriptionProperty<Vector2> onAxisShift)
    {
        _onAxisShift = onAxisShift;
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        CheckAxisShift(deltaTime);
    }

    #endregion

    #region Methods

    private void CheckAxisShift(float deltaTime)
    {
        var axisShift =
            new Vector2(
                Input.GetAxis(InputAxises.HORIZONTAL),
                Input.GetAxis(InputAxises.VERTICAL));

        if (axisShift.magnitude == 0) return;

        _onAxisShift.Value = axisShift * deltaTime;
    }

    #endregion
}
