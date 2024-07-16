using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class TeleportationObserver : MonoBehaviour
{
    public delegate void TeleportationDoneHandler();
    public static event TeleportationDoneHandler OnTeleportationDone;

    public TeleportationProvider teleportationProvider;
    private LocomotionPhase lastLocomotionPhase;

    void Awake()
    {
        if (teleportationProvider == null)
        {
            Debug.LogError("TeleportationProvider component not found on this GameObject.");
            return;
        }

        lastLocomotionPhase = teleportationProvider.locomotionPhase;
    }

    void Update()
    {
        if (teleportationProvider.locomotionPhase == LocomotionPhase.Done && lastLocomotionPhase != LocomotionPhase.Done)
        {
            OnTeleportationDone?.Invoke();
            Debug.Log("TeleportationProvider Done.");
        }

        lastLocomotionPhase = teleportationProvider.locomotionPhase;
    }
}
