using UnityEngine;

public abstract class ControllerSpawner<TController, TView> : Spawner<GameObject, TController>
    where TController : IController
    where TView : MonoBehaviour
{
    #region Fields

    protected Transform _root;

    protected TView _view;
    protected TController _controller;

    #endregion

    #region Constructors

    public ControllerSpawner(GameObject prefab, Transform root) : base(prefab)
    {
        _root = root;
    }

    public ControllerSpawner(GameObject prefab, Transform root, int heatQuantity) : this(prefab, root)
    {
        Heat(heatQuantity);
    }

    #endregion

    #region Methods

    public override TController Pop()
    {
        _controller = base.Pop();

        _view = GetView(_controller);

        EnableView(_view);

        return _controller;
    }

    public override void Push(TController instance)
    {
        _view = GetView(instance);

        DisableView(_view);

        base.Push(instance);
    }

    protected override TController Create()
    {
        _view = Object.Instantiate(_template).GetComponent<TView>();

        return CreateController(_view);
    }

    protected abstract TView GetView(TController controller);
    protected abstract void EnableView(TView view);
    protected abstract void DisableView(TView view);
    protected abstract TController CreateController(TView view);

    #endregion
}