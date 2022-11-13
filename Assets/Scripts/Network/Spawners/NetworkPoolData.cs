public class NetworkPoolData<TData, TPool> : PoolData<TData, TPool>
    where TData : IPoolData, IGameData
{
    #region Constructors

    public NetworkPoolData(TData data, TPool pool) : base(data, pool) {}

    #endregion
}
