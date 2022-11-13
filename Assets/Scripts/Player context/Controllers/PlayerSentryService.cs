using UnityEngine;
using Photon.Pun;

public class PlayerSentryService : PhotonSentryService
{
    #region Fields

    private object[] PARAMETERS = new object[2];
    
    #endregion

    #region Base methods

    protected override void NetworkInitialize(PhotonSentry sentry, object[] customData)
    {
        sentry.transform.position = (Vector3)    customData[0];
        sentry.transform.rotation = (Quaternion) customData[1];
    }

    [PunRPC]
    protected override void NetworkInstantiate_RPC(string prefab, object[] customData)
    {
        base.NetworkInstantiate_RPC(prefab, customData);
    }
    
    #endregion

    #region Methods

    public void NetworkInstantiate(
        string prefab,
        GameObject go,
        Vector3 position,
        Quaternion rotation)
    {
        AddSentry(go);

        PARAMETERS[0] = position;
        PARAMETERS[1] = rotation;

        base.NetworkInstantiate(prefab, PARAMETERS);
    }

    #endregion
}
