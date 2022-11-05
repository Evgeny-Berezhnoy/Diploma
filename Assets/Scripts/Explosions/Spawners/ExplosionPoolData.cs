using UnityEngine;

public sealed class ExplosionPoolData : PoolData<ExplosionData, Transform>
{
    #region Constructors

    public ExplosionPoolData(ExplosionData data, Transform pool) : base(data, pool) {}

    #endregion
}
