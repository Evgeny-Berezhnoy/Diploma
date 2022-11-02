using UnityEngine;

public class AudiosourceView : MonoBehaviour
{
    #region Fields

    [SerializeField] private AudioSource _audiosource;

    #endregion

    #region Properties

    public AudioSource Audiosource => _audiosource;

    #endregion
}