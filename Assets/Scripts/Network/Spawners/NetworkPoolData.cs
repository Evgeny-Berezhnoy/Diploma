using Photon.Pun;

public class NetworkPoolData<TData> : PoolData<TData, PhotonView>
    where TData : IPoolData, IGameData
{
    #region Constructors

    public NetworkPoolData(TData data, PhotonView pool) : base(data, pool) {}

    #endregion
}
