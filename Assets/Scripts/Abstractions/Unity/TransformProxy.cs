using UnityEngine;

public class TransformProxy
{
    #region Fields

    public Vector3 Position;
    public Quaternion Rotation;

    #endregion

    #region Constructors

    public TransformProxy()
    {
        Position = Vector3.zero;
        Rotation = Quaternion.identity;
    }

    public TransformProxy(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }

    public TransformProxy(Transform transform)
    {
        Position = transform.position;
        Rotation = transform.rotation;
    }

    #endregion

    #region Operators

    public static implicit operator TransformProxy(Transform v) => new TransformProxy(v.position, v.rotation);
    
    #endregion
}
