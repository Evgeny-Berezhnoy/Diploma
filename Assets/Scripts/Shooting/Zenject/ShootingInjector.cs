﻿using UnityEngine;
using Zenject;

public class ShootingInjector : MonoInstaller
{
    #region Fields

    [SerializeField] private Transform _defaultTarget;

    #endregion

    #region Base methods

    public override void InstallBindings()
    {
        Container.BindSubscriptionProperty<ProjectileController>("Shooting : onAddController");
        Container.BindSubscriptionProperty<ProjectileController>("Shooting : onRemoveController");
        Container.BindSubscriptionProperty<ProjectileLaunchData>("Shooting : onLaunch");
        Container.BindSubscriptionProperty<ProjectilePhysicsController>("Shooting : onHit");
        Container.BindSubscriptionProperty<ProjectileView>("Shooting : OnRemoteHit");
        Container.BindSubscriptionSurvey<Transform>("Shooting : TargetSurvey", true);
        Container.BindComponent(_defaultTarget, "Shooting : DefaultTarget");
    }

    #endregion
}
