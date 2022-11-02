using UnityEngine;

public class NetworkSpawner : Spawner<IGameData, GameObject>
{
    #region Fields

    protected TransformProxy _root;

    #endregion

    #region Constructors

    public NetworkSpawner(IGameData prefab, TransformProxy root) : base(prefab)
    {
        _root = root;
    }

    public NetworkSpawner(IGameData prefab, TransformProxy root, int heatQuantity) : this(prefab, root)
    {
        Heat(heatQuantity);
    }

    #endregion

    #region Methods

    public override GameObject Pop()
    {
        var instance = base.Pop();

        instance.gameObject.SetActive(true);

        return instance;
    }

    public override void Push(GameObject instance)
    {
        base.Push(instance);

        instance
            .transform
            .SetPositionAndRotation(
                _root.Position,
                _root.Rotation);

        instance.gameObject.SetActive(false);
    }

    protected override GameObject Create()
    {
        return PhotonCore.Instance.InstantiateInstance(_template.Path);
    }

    #endregion
}