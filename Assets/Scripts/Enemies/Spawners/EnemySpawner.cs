using UnityEngine;

public class EnemySpawner : NetworkControllerSpawner<EnemyController, EnemyView>
{
    #region Fields

    private EnemyPoolData _poolData;

    #endregion

    #region Constructors

    public EnemySpawner(
        EnemyPoolData poolData,
        int bufferQuantity)
    :
    base(
        poolData.Data.Path,
        bufferQuantity,
        poolData.Pool)
    {
        _poolData = poolData;
    }

    public EnemySpawner(
        EnemyPoolData poolData,
        int bufferQuantity,
        int heatQuantity)
    :
    this(
        poolData,
        bufferQuantity)
    {
        Heat(heatQuantity);
    }

    #endregion

    #region Base methods

    public override EnemyController Pop()
    {
        var controller = base.Pop();

        controller.GunController.Reset();
        controller.HealthController.Recover();

        return controller;
    }

    public override void Push(EnemyController instance)
    {
        base.Push(instance);
    }

    protected override EnemyView GetView(EnemyController controller)
    {
        return controller.View;
    }

    protected override void EnableView(EnemyView view)
    {
        view.Spawn();
    }

    protected override void DisableView(EnemyView view)
    {
        view.Despawn();
    }

    protected override EnemyController CreateController(GameObject go)
    {
        var view = go.GetComponent<EnemyView>();

        view.Pool = _root.ViewID;

        var moveController          = new EnemyMoveController(view.transform, _poolData.Data.Speed, _poolData.Data.OverwatchTime);
        var targetController        = new EnemyTargetController(_poolData.Data.TargetRotationSpeed, view.transform);

        var projectilePoolData      = new ProjectilePoolData(LayerMask.GetMask(Layers.PLAYER), _poolData.Data.ProjectileData, view.ProjectilePool);
        var projectileLaunchData    = new ProjectileLaunchData(projectilePoolData, view.Muzzles);

        var gunController           = new EnemyGunController(projectileLaunchData, _poolData.OnLaunchSubscription, _poolData.Data.WeaponRechargeTime);

        var healthController        = new HealthController(_poolData.Data.Health);

        healthController.AddDeathListener(view.Kill);
        
        var contactScaner =
            new ContactScaner(
                view.HitCollider,
                LayerMask.GetMask(Layers.PLAYER_PROJECTILE),
                true,
                _poolData.ProjectileHitContactsAmount);

        var enemyController =
            new EnemyController(
                _template,
                view,
                moveController,
                targetController,
                gunController,
                healthController);

        _poolData.Disposer.Subscribe(gunController.Dispose);
        _poolData.Disposer.Subscribe(healthController.Dispose);

        return enemyController;
    }

    #endregion
}
