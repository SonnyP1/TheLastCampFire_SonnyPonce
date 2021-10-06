using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class InteractableScript : MonoBehaviour
{
    GameObject Owner;
    public virtual void Interact()
    {
        Debug.Log("Interaction is happeningggg!");
        if (Owner)
            Debug.Log("Owner name is: " + Owner.name);
        else
            Debug.Log("Owner is null");
    }
    public void SetOwnerGameObject(GameObject newOwner)
    {
        Owner = newOwner;
    }
    public GameObject GetOwnerGameObject() { return Owner; }
}
