using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalManager : MonoBehaviour
{
    public UnityEvent OnPlayerEnterAnchor;
    public UnityEvent OnPlayerExitAnchor;
    public Transform trackerAnchor;
    public GameObject[] trackedVisual;
    private void Start()
    {
        PlayerManager.Instance.PlayerInPhyBoundary_EventHandler += OnPlayerInPhyBoundary;
        // TransitionManager.Instance.UpdateSceneTransitionFilter(false);
    }

    private void OnPlayerInPhyBoundary(object sender, PlayerManager.PlayerBoundStateEventArgs e)
    {
        PhysicalManager playerPS = PlayerManager.Instance.selfBoundary.physicalSpace.GetComponent<PhysicalManager>();
        
        if (playerPS == this)
        {
            FluidSpaceUtils.ShowListItem(trackedVisual,true);
            if (e.isInBoundary && playerPS == this)
            {
                OnPlayerEnterAnchor?.Invoke();
            }
            else
            {
                OnPlayerExitAnchor?.Invoke();
            }
        }
        else
        {
            FluidSpaceUtils.ShowListItem(trackedVisual,false);
        }
    }
}
