using UnityEngine;

public class ViewSpawner<TView> : Spawner<GameObject, TView>
        where TView : MonoBehaviour
{
    #region Fields

    protected Transform _root;

    #endregion
    
    #region Constructors

    public ViewSpawner(GameObject prefab, Transform root) : base(prefab)
    {
        _root = root;
    }

    public ViewSpawner(GameObject prefab, Transform root, int heatQuantity) : this(prefab, root)
    {
        Heat(heatQuantity);
    }

    #endregion

    #region Methods

    public override TView Pop()
    {
        var instance = base.Pop();

        instance.transform.SetParent(null);
        instance.gameObject.SetActive(true);

        return instance;
    }

    public override void Push(TView instance)
    {
        base.Push(instance);

        instance.transform.position = _root.position;
        instance.transform.SetParent(_root);
        instance.gameObject.SetActive(false);
    }

    protected override TView Create()
    {
        var prefabClone = Object.Instantiate(_template);
        var view = prefabClone.GetComponent<TView>();

        return view;
    }

    #endregion
}