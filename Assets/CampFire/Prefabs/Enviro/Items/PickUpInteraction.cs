using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpInteraction : InteractableScript
{
    protected bool isPickUp;
    public override void Interact()
    {
        if (!isPickUp)
        {
            PickUpItem();
        }
        else
        {
            DropItem();
        }

    }

    public virtual void DropItem()
    {
        gameObject.transform.parent = null;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        isPickUp = false;
    }

    public virtual void PickUpItem()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;

        float ZAxisDot = Vector3.Dot((transform.position - GetOwnerGameObject().transform.position).normalized, GetOwnerGameObject().transform.forward);

        if (ZAxisDot > 0)
        {
            gameObject.transform.position = GetOwnerGameObject().transform.position;
            gameObject.transform.rotation = GetOwnerGameObject().transform.rotation;
            gameObject.transform.parent = GetOwnerGameObject().transform;
            isPickUp = true;
        }

    }
}
