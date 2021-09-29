using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InteractableScript : MonoBehaviour
{
    GameObject Owner;
    public virtual void Interact()
    {
        Debug.Log("Interaction is happeningggg!");
    }
    public void SetOwnerGameObject(GameObject newOwner)
    {
        Owner = newOwner;
    }
    public GameObject GetOwnerGameObject() { return Owner; }
}
