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

    private void Start()
    {
        EventBus<OnIngredientAddedToMixEvent>.AddListener(new EventBinding<OnIngredientAddedToMixEvent>(OnIngredientAddedToMix));
        EventBus<OnIngredientPickupEvent>.AddListener(new EventBinding<OnIngredientPickupEvent>(OnIngredientPickup));
        EventBus<OnRecipeBookOpenedEvent>.AddListener(new EventBinding<OnRecipeBookOpenedEvent>(OnRecipeBookOpened));
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
    
}
