using UnityEngine;

public interface IExplosive
{
    #region Properties

    ISubscriptionProperty<Transform> OnExplosion { set; }

    #endregion
}
