public sealed class ProjectilePoolData : NetworkPoolData<ProjectileData, PhotonSentry>
{
    #region Fields

    public readonly int LayerMask;
    
    #endregion

    #region Constructors

    public ProjectilePoolData(ProjectileData data, PhotonSentry pool, int layerMask) : base(data, pool)
    {
        LayerMask = layerMask;
    }

    #endregion
}
