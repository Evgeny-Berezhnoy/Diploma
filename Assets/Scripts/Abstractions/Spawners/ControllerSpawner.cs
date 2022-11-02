using UnityEngine;

public abstract class ControllerSpawner<TController, TView> : Spawner<GameObject, TController>
    where TController : IController
    where TView : MonoBehaviour
{
    #region Fields

    protected Transform _root;

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
        var instance = base.Pop();

        var view = GetView(instance);

        view.transform.SetParent(null);
        view.gameObject.SetActive(true);

        return instance;
    }

    public override void Push(TController instance)
    {
        var view = GetView(instance);

        view.transform.position = _root.position;
        view.transform.SetParent(_root);
        view.gameObject.SetActive(false);

        base.Push(instance);
    }

    protected override TController Create()
    {
        var prefabClone = Object.Instantiate(_template);
        var view        = prefabClone.GetComponent<TView>();

        return CreateController(view);
    }

    protected abstract TView GetView(TController controller);
    protected abstract TController CreateController(TView view);

    #endregion
}