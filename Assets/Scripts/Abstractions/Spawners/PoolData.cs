public class PoolData<TData, TPool>
    where TData : IPoolData
{
    #region Fields

    public readonly TData Data;
    public readonly TPool Pool;

    #endregion

    #region Constructors

    public PoolData(TData data, TPool pool)
    {
        Data = data;
        Pool = pool;
    }

    #endregion
}
