using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalPanel : MonoBehaviour, IInteractable
{
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }
    public static UnityAction CliclOnPortalPanel;
  

    public void Interact(ActionController interactor, out bool interactSuccessful)
    {
        CliclOnPortalPanel?.Invoke();
        interactSuccessful = true;
    }
    public void EndInteraction()
    {

    }

}
