using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractComponent : MonoBehaviour
{
    List<InteractableScript> interactables = new List<InteractableScript>();
    private void OnTriggerEnter(Collider other)
    {
        InteractableScript otherAsInteractable = other.GetComponent<InteractableScript>();
        if(otherAsInteractable)
        {
            if(!interactables.Contains(otherAsInteractable))
            {
                interactables.Add(otherAsInteractable);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableScript otherAsInteractable = other.GetComponent<InteractableScript>();
        if (otherAsInteractable)
        {
            if (interactables.Contains(otherAsInteractable))
            {
                interactables.Remove(otherAsInteractable);
            }
        }
    }

    public void Interact()
    {
        InteractableScript closestInteractable = GetClosestInteractable();
        if(closestInteractable != null)
        {
            closestInteractable.Interact();
        }
    }

    InteractableScript GetClosestInteractable()
    {
        InteractableScript cloesetInteract = null;
        if(interactables.Count ==0)
        {
            return cloesetInteract;
        }

        float closestDist = float.MaxValue;
        foreach(var interactablesItem in interactables)
        {
            float Dist = Vector3.Distance(transform.position,interactablesItem.transform.position);
            if(Dist < closestDist)
            {
                cloesetInteract = interactablesItem;
                closestDist = Dist;
            }
        }
        return cloesetInteract;
    }
}

