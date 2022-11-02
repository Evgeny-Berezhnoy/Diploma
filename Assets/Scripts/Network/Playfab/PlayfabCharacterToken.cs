using System;
using UnityEngine;

[Serializable]
public class PlayfabCharacterToken
{
    #region Fields

    [SerializeField] private string _itemID;
    [SerializeField] private CharacterData _character;
    [SerializeField] private int _price;
    [SerializeField] private PlayfabCharacterAcquisitionCondition _acquisitionCondition;

    #endregion

    #region Properties

    public string ItemID => _itemID;
    public CharacterData Character => _character;
    public int Price => _price;
    public PlayfabCharacterAcquisitionCondition AcquisitionCondition => _acquisitionCondition;

    #endregion
}
