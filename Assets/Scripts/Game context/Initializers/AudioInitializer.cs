using UnityEngine;
using Zenject;

public class AudioInitializer
{
    #region Injected methods

    [Inject]
    private void Initialize(
        // GameContextInjector
        [Inject(Id = "GameContext : onDefeat")] ISubscriptionProperty onDefeat,
        [Inject(Id = "GameContext : onVictory")] ISubscriptionProperty onVictory,
        [Inject(Id = "GameContext : onRetry")] ISubscriptionProperty onRetry,
        // AudioSettings
        [Inject(Id = "AudioSettings : Audiosource")] AudiosourceView audiosource,
        [Inject(Id = "AudioSettings : LevelSoundtrack")] AudioClip levelSoundtrack,
        [Inject(Id = "AudioSettings : DefeatSoundtrack")] AudioClip defeatSoundtrack,
        [Inject(Id = "AudioSettings : VictorySoundtrack")] AudioClip victorySoundtrack)
    {
        var levelAudiosource                = Object.Instantiate(audiosource);
        var levelAudiosourceController      = new AudiosourceController(levelAudiosource, true, levelSoundtrack, true);

        var finishGameAudiosource           = Object.Instantiate(audiosource);
        var finishGameAudiosourceController = new AudiosourceController(finishGameAudiosource, false);

        onDefeat.Subscribe(levelAudiosourceController.Stop);
        onDefeat.Subscribe(() => finishGameAudiosourceController.Play(defeatSoundtrack));

        onVictory.Subscribe(levelAudiosourceController.Stop);
        onVictory.Subscribe(() => finishGameAudiosourceController.Play(victorySoundtrack));

        onRetry.Subscribe(levelAudiosourceController.Play);
        onRetry.Subscribe(finishGameAudiosourceController.Stop);
    }

    #endregion
}
