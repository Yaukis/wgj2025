using Ami.BroAudio;
using UnityEngine;
using Utils;
using Utils.EventBus;

public class AudioManager : MonoSingleton<AudioManager>
{
    [Header("Sound Effects")]
    [SerializeField] private SoundID calderoSFX;
    [SerializeField] private SoundID grabSFX;
    [SerializeField] private SoundID menuOpenSFX;
    [SerializeField] private SoundID errorSFX;
    [SerializeField] private SoundID flushSFX;

    [Header("Background Music")]
    [SerializeField] private SoundID bkgMusic;
    [SerializeField] private float pitchShiftStep = 0.01f; // amount to decrease pitch each interval
    [SerializeField] private float pitchShiftInterval = 300f; // in milliseconds
    
    private float _timer = 0f;
    private bool _hardmodeStarted = false;
    private float _currentPitch = 1f; // initial pitch

    private void Start()
    {
        EventBus<OnIngredientAddedToMixEvent>.AddListener(new EventBinding<OnIngredientAddedToMixEvent>(OnIngredientAddedToMix));
        EventBus<OnIngredientPickupEvent>.AddListener(new EventBinding<OnIngredientPickupEvent>(OnIngredientPickup));
        EventBus<OnRecipeBookOpenedEvent>.AddListener(new EventBinding<OnRecipeBookOpenedEvent>(OnRecipeBookOpened));
        EventBus<OnHardmodeStartedEvent>.AddListener(new EventBinding<OnHardmodeStartedEvent>(OnHardmodeStarted));
        EventBus<OnHardmodeFailedEvent>.AddListener(new EventBinding<OnHardmodeFailedEvent>(OnHardmodeFailed));
        EventBus<OnOrderFailedEvent>.AddListener(new EventBinding<OnOrderFailedEvent>(OnOrderFailed));
        EventBus<OnPotionResetEvent>.AddListener(new EventBinding<OnPotionResetEvent>(OnPotionReset));
    }

    private void Update()
    {

        // each interval decrease the pitch by pitchShiftStep
        if (_hardmodeStarted)
        {
            // make broaudio pitch be the same as pitchStart
            
            _timer += Time.deltaTime * 1000f; // Convert to milliseconds
            if (_timer >= pitchShiftInterval)
            {
                _currentPitch -= pitchShiftStep;
                BroAudio.SetPitch(bkgMusic, _currentPitch);
                _timer = 0f;
            }
        
        }
    }


    private void OnDestroy()
    {
        EventBus<OnIngredientAddedToMixEvent>.RemoveListener(new EventBinding<OnIngredientAddedToMixEvent>(OnIngredientAddedToMix));
        EventBus<OnIngredientPickupEvent>.RemoveListener(new EventBinding<OnIngredientPickupEvent>(OnIngredientPickup));
        EventBus<OnRecipeBookOpenedEvent>.RemoveListener(new EventBinding<OnRecipeBookOpenedEvent>(OnRecipeBookOpened));
        EventBus<OnHardmodeStartedEvent>.RemoveListener(new EventBinding<OnHardmodeStartedEvent>(OnHardmodeStarted));
        EventBus<OnHardmodeFailedEvent>.RemoveListener(new EventBinding<OnHardmodeFailedEvent>(OnHardmodeFailed));
        EventBus<OnOrderFailedEvent>.RemoveListener(new EventBinding<OnOrderFailedEvent>(OnOrderFailed));
        EventBus<OnPotionResetEvent>.RemoveListener(new EventBinding<OnPotionResetEvent>(OnPotionReset));
    }
    
    private void OnIngredientAddedToMix(OnIngredientAddedToMixEvent evt)
    {
        if (calderoSFX == -1) return;
        
        BroAudio.Play(calderoSFX);
    }
    
    private void OnIngredientPickup(OnIngredientPickupEvent evt)
    {
        if (grabSFX == -1) return;
        
        BroAudio.Play(grabSFX);
    }
    
    private void OnRecipeBookOpened(OnRecipeBookOpenedEvent evt)
    {
        if (menuOpenSFX == -1) return;
        
        BroAudio.Play(menuOpenSFX);
    }

    private void OnHardmodeStarted(OnHardmodeStartedEvent evt)
    {
        _hardmodeStarted = true;
    }

    private void OnHardmodeFailed(OnHardmodeFailedEvent evt)
    {
        _hardmodeStarted = false;
        _currentPitch = 1f; // reset pitch to initial value
        BroAudio.SetPitch(bkgMusic, _currentPitch);
        _timer = 0f; // reset timer
    }
    
    private void OnOrderFailed(OnOrderFailedEvent evt)
    {
        if (errorSFX == -1) return;
        
        BroAudio.Play(errorSFX);
    }

    private void OnPotionReset(OnPotionResetEvent evt)
    {
        if (flushSFX == -1) return;
        
        BroAudio.Play(flushSFX);
    }
    
    public void SetVolume(float volume)
    {
        BroAudio.SetVolume(volume);
        PlayerPrefs.SetFloat("masterVolume", volume);
        PlayerPrefs.Save();
    }
    
    public void SetMusicVolume(float volume)
    {
        BroAudio.SetVolume(BroAudioType.Music, volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        BroAudio.SetVolume(BroAudioType.SFX, volume);
        BroAudio.SetVolume(BroAudioType.Ambience, volume);
        PlayerPrefs.SetFloat("sfxVolume", volume);
        PlayerPrefs.Save();
    }
}
