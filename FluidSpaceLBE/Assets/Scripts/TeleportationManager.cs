using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    public XRRayInteractor xRRayInteractor;
    public TeleportationProvider teleportationProvider;
    public static event Action<TeleportRequest> OnTeleportRequest;
    public static event Action<Vector3, Quaternion> OnTeleportationDone;

    private TeleportRequest lastRequest;
    private bool teleportInProgress = false;
    private void Update()
    {
        if (teleportationProvider == null) return;

        // 反射获取 currentRequest 和 validRequest
        var currentRequestField = typeof(TeleportationProvider).GetProperty("currentRequest", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var validRequestField = typeof(TeleportationProvider).GetProperty("validRequest", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (currentRequestField != null && validRequestField != null)
        {
            var currentRequest = (TeleportRequest)currentRequestField.GetValue(teleportationProvider);
            var validRequest = (bool)validRequestField.GetValue(teleportationProvider);

            if (validRequest && !teleportInProgress)
            {
                lastRequest = currentRequest;
                teleportInProgress = true;
                OnTeleportRequest?.Invoke(currentRequest);
                Debug.Log($"Teleport Request: Destination Position = {currentRequest.destinationPosition}, Destination Rotation = {currentRequest.destinationRotation}");
            }

            if (teleportInProgress && !validRequest)
            {
                OnTeleportationDone?.Invoke(lastRequest.destinationPosition, lastRequest.destinationRotation);
                Debug.Log($"Teleportation Done: New Position = {lastRequest.destinationPosition}, New Rotation = {lastRequest.destinationRotation}");
                teleportInProgress = false;
            }
        }
    }
}
