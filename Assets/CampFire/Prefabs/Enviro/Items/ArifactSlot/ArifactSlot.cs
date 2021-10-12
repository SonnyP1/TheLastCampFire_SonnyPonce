using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArifactSlot : MonoBehaviour
{
    [SerializeField] Transform ArtifactSlotTrans;
    [SerializeField] GameObject ToggleObject;
    public void OnArtifactLeft()
    {
        ToggleObject.GetComponent<Toggleable>().ToggleOff();
    }

    public void OnArtifactPlaced()
    {
        ToggleObject.GetComponent<Toggleable>().ToggleOn();
    }

    public Transform GetSlotTransform()
    {
        return ArtifactSlotTrans;
    }
}
