public class ExplosionSpawner : ControllerSpawner<ExplosionController, ExplosionView>
{
    #region Fields

    private ExplosionPoolData _poolData;

    #endregion
    
    #region Constructors

    public ExplosionSpawner(
        ExplosionPoolData poolData)
    :
    base(
        poolData.Data.Prefab,
        poolData.Pool)
    {
        _poolData = poolData;
    }

    public ExplosionSpawner(
        ExplosionPoolData poolData,
        int heatQuantity)
    :
    this(
        poolData)
    {
        Heat(heatQuantity);
    }

    #endregion

    #region Base methods

    public override ExplosionController Pop()
    {
        _controller = base.Pop();

        _controller.Reset();
        
        return _controller;
    }

    protected override ExplosionView GetView(ExplosionController controller)
    {
        return controller.View;
    }

    protected override void EnableView(ExplosionView view)
    {
        view.Spawn();

        view.transform.SetParent(null);
    }

    protected override void DisableView(ExplosionView view)
    {
        view.Despawn();

        view.transform.position = _root.position;
        view.transform.SetParent(_root);
    }

    protected override ExplosionController CreateController(ExplosionView view)
    {
        _controller = new ExplosionController(view, _poolData.Data.SpriteTransitionTime);

        return _controller;
    }

    #endregion
}
