using UnityEngine;

public class ProjectileSpawner : NetworkControllerSpawner<ProjectileController, ProjectileView>
{
    #region Fields

    private int _hitContactsAmount;
    private ProjectilePoolData _poolData;

    #endregion

    #region Observers

    private ISubscriptionProperty<ProjectilePhysicsController> _onHit;

    #endregion

    #region Constructors

    public ProjectileSpawner(
        ProjectilePoolData poolData,
        int hitContactsAmount,
        int bufferQuantity)
    :
    base(
        poolData.Data.Path,
        bufferQuantity,
        poolData.Pool)
    {
        _poolData           = poolData;
        _hitContactsAmount  = hitContactsAmount;
    }

    public ProjectileSpawner(
        ProjectilePoolData poolData,
        int bufferQuantity,
        int hitContactsAmount,
        int heatQuantity)
    :
    this(
        poolData,
        hitContactsAmount,
        bufferQuantity)
    {
        Heat(heatQuantity);
    }

    #endregion

    #region Base methods

    public override ProjectileController Pop()
    {
        var controller = base.Pop();

        controller.Reset();

        return controller;
    }

    public override void Push(ProjectileController instance)
    {
        var view = GetView(instance);

        DisableView(view);
        
        base.Push(instance);
    }

    protected override ProjectileView GetView(ProjectileController controller)
    {
        return controller.View;
    }

    protected override void EnableView(ProjectileView view)
    {
        view.Launch();
    }

    protected override void DisableView(ProjectileView view)
    {
        view.Hide();
    }

    protected override ProjectileController CreateController(GameObject go)
    {
        var view = go.GetComponent<ProjectileView>();

        view.Pool   = _root.ViewID;
        view.Damage = _poolData.Data.Damage;

        var moveController =
            new ProjectileMoveController(
                view.transform,
                _poolData.Data.Speed,
                _poolData.Data.LifeTime);

        var hitScaner =
            new ContactScaner(
                view.Collider,
                _poolData.LayerMask,
                true,
                _hitContactsAmount);

        var physicsController =
            new ProjectilePhysicsController(
                view,
                hitScaner);

        var controller =
            new ProjectileController(
                view,
                _root,
                moveController,
                physicsController);

        return controller;
    }

    #endregion
}
