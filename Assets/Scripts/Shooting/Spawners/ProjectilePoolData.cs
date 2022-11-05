using Photon.Pun;

public sealed class ProjectilePoolData : NetworkPoolData<ProjectileData>
{
    #region Fields

    public readonly int LayerMask;
    
    #endregion

    #region Constructors

    public ProjectilePoolData(int layerMask, ProjectileData data, PhotonView pool) : base(data, pool)
    {
        LayerMask = layerMask;
    }

    #endregion
}
