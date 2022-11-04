using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Audio settings", menuName = "Game/Audio settings")]
public class AudioSettings : ScriptableObjectInstaller
{
    #region Fields

    [SerializeField] private GameObject _prefab;
    [SerializeField] private AudioClip _levelSoundtrack;
    [SerializeField] private AudioClip _defeatSoundtrack;
    [SerializeField] private AudioClip _victorySoundtrack;
    
    #endregion

    #region Base methods

    public override void InstallBindings()
    {
        Container
            .Bind<AudiosourceView>()
            .WithId("AudioSettings : Audiosource")
            .FromComponentOn(_prefab)
            .AsCached();

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
