using Photon.Pun;

public class PoolData<T>
    where T : IGameData
{
    #region Fields

    public readonly T Data;
    public readonly PhotonView Pool;

    #endregion

    #region Constructors

    public PoolData(T data, PhotonView pool)
    {
        Data = data;
        Pool = pool;
    }

    #endregion
}
