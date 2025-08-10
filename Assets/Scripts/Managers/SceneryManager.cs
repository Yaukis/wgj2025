using System;
using UnityEngine;
using Utils.EventBus;

public class SceneryManager : MonoBehaviour
{
    [SerializeField] private GameObject normalModeScenery;
    [SerializeField] private GameObject hardModeScenery;

    private void Start()
    {
        if (normalModeScenery == null || hardModeScenery == null)
        {
            Debug.LogError("SceneryManager: Scenery objects not assigned.");
            return;
        }

        EventBus<OnHardmodeStartedEvent>.AddListener(new EventBinding<OnHardmodeStartedEvent>(OnHardmodeStarted));
        EventBus<OnHardmodeFailedEvent>.AddListener(new EventBinding<OnHardmodeFailedEvent>(OnHardmodeFailed));
    }
    
    private void OnDestroy()
    {
        EventBus<OnHardmodeStartedEvent>.RemoveListener(new EventBinding<OnHardmodeStartedEvent>(OnHardmodeStarted));
        EventBus<OnHardmodeFailedEvent>.RemoveListener(new EventBinding<OnHardmodeFailedEvent>(OnHardmodeFailed));
    }
    
    private void OnHardmodeStarted(OnHardmodeStartedEvent evt)
    {
        if (normalModeScenery) normalModeScenery.SetActive(false);
        if (hardModeScenery) hardModeScenery.SetActive(true);
        Debug.Log("Hardmode scenery activated.");
    }
    
    private void OnHardmodeFailed(OnHardmodeFailedEvent evt)
    {
        if (normalModeScenery) normalModeScenery.SetActive(true);
        if (hardModeScenery) hardModeScenery.SetActive(false);
        Debug.Log("Hardmode scenery deactivated.");
    }
}