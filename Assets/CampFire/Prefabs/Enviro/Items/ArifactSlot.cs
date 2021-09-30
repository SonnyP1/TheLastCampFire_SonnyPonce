using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArifactSlot : MonoBehaviour
{
    [SerializeField] Transform ArtifactSlotTrans;
    [SerializeField] Platform platformToMove;
    public void OnArtifactLeft()
    {
        print("ArtifactLeft!");
        platformToMove.MoveTo(platformToMove.StartTrans);
    }

    public void OnArtifactPlaced()
    {
        print("ArtifactPlaced");
        platformToMove.MoveTo(platformToMove.EndTrans);
    }

    public Transform GetSlotTransform()
    {
        return ArtifactSlotTrans;
    }
}
