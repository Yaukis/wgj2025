using System;
using Ami.BroAudio;
using UnityEngine;
using Utils;
using Utils.EventBus;


public class AudioManager : MonoSingleton<AudioManager>
{

    [SerializeField] private SoundID calderoSFX;
    [SerializeField] private SoundID grabSFX;
    [SerializeField] private SoundID menuOpenSFX;
    [SerializeField] private SoundID errorSFX;

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
    }
    
    private void OnIngredientAddedToMix(OnIngredientAddedToMixEvent evt)
    {
        BroAudio.Play(calderoSFX);
    }
    
    private void OnIngredientPickup(OnIngredientPickupEvent evt)
    {
        BroAudio.Play(grabSFX);
    }
    
    private void OnRecipeBookOpened(OnRecipeBookOpenedEvent evt)
    {
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
        BroAudio.Play(errorSFX);
    }
}
