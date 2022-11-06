using UnityEngine;

public class SpecialEffectSpawner : ControllerSpawner<SpecialEffectController, SpecialEffectView>
{
    #region Constructors

    public SpecialEffectSpawner(GameObject prefab, Transform root) : base(prefab, root) {}
    public SpecialEffectSpawner(GameObject prefab, Transform root, int heatQuantity) : base(prefab, root, heatQuantity) {}

    #endregion

    #region Base methods

    protected override SpecialEffectView GetView(SpecialEffectController controller)
    {
        return controller.View;
    }

    protected override void EnableView(SpecialEffectView view)
    {
        view.transform.SetParent(null);
    }

    protected override void DisableView(SpecialEffectView view)
    {
        view.transform.position = _root.position;
        view.transform.SetParent(_root);
    }

    protected override SpecialEffectController CreateController(SpecialEffectView view)
    {
        _controller = new SpecialEffectController(view);

        return _controller;
    }

    #endregion
}
