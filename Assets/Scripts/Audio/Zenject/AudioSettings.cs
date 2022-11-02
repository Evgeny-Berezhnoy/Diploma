using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Audio settings", menuName = "Game/Audio settings")]
public class AudioSettings : ScriptableObjectInstaller
{
    #region Fields

    [SerializeField] private AudiosourceView _view;
    [SerializeField] private AudioClip _levelSoundtrack;
    [SerializeField] private AudioClip _defeatSoundtrack;
    [SerializeField] private AudioClip _victorySoundtrack;
    
    #endregion

    #region Base methods

    public override void InstallBindings()
    {
        Container.BindMonoBehaviour(_view, "AudioSettings : Audiosource");

        Container
            .Bind<AudioClip>()
            .WithId("AudioSettings : LevelSoundtrack")
            .FromInstance(_levelSoundtrack);

        Container
            .Bind<AudioClip>()
            .WithId("AudioSettings : DefeatSoundtrack")
            .FromInstance(_defeatSoundtrack);

        Container
            .Bind<AudioClip>()
            .WithId("AudioSettings : VictorySoundtrack")
            .FromInstance(_victorySoundtrack);
    }

    #endregion
}
