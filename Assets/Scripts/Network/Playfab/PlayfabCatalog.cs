using System;
using UnityEngine;


[CreateAssetMenu(fileName = "Playfab catalog", menuName = "PlayFab/Custom/Catalog")]
public class PlayfabCatalog : ScriptableObject
{
    #region Fields

    [SerializeField] private string _version;
    [SerializeField] private string _currency;

    [SerializeField] private PlayfabCharacterToken[] _characters;

    #endregion

    #region Properties

    public string Version => _version;
    public string Currency => _currency;
    public PlayfabCharacterToken DefaultCharacterToken
    {
        get
        {
            if(_characters.Length == 0)
            {
                throw new Exception("Character tokens are empty.");
            };

            return _characters[0];
        }
    }

    public PlayfabCharacterToken[] Characters => _characters;

    #endregion
}
