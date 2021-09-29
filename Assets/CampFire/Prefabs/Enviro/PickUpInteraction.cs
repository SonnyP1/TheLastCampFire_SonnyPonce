using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInteraction : InteractableScript
{
    public override void Interact()
    {
        float ZAxisDot = Vector3.Dot((transform.position - GetOwnerGameObject().transform.position).normalized, GetOwnerGameObject().transform.forward);

        if (ZAxisDot > 0)
        {
            gameObject.transform.position = GetOwnerGameObject().transform.position;
            gameObject.transform.parent = GetOwnerGameObject().transform;
        }
        
    }
}
