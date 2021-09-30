using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : PickUpInteraction
{
    [SerializeField] float DropDownSlotSearchRadius = 0.2f;
    ArifactSlot CurrentSlot = null;

    private void Start()
    {
        DropItem();
    }
    public override void PickUpItem()
    {
        base.PickUpItem();
        if(CurrentSlot)
        {
            CurrentSlot.OnArtifactLeft();
            CurrentSlot = null;
        }
    }

    public override void DropItem()
    {
        ArifactSlot slot = GetArifactSlot();
        if (slot != null)
        {
            slot.OnArtifactPlaced();
            transform.parent = null;
            transform.rotation = slot.GetSlotTransform().rotation;
            transform.position = slot.GetSlotTransform().position;
            CurrentSlot = slot;
            isPickUp = false;
        }
        else
        {
            base.DropItem();
        }
    }

    ArifactSlot GetArifactSlot()
    {
        Collider[] Cols = Physics.OverlapSphere(transform.position, DropDownSlotSearchRadius);
        foreach(Collider col in Cols)
        {
            ArifactSlot slot = col.GetComponent<ArifactSlot>();
            if(slot != null)
            {
                return slot;
            }
        }
        return null;
    }
}
