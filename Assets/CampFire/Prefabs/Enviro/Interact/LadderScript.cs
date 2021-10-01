using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour
{
    [SerializeField] Transform SnappingPointBottom;
    [SerializeField] Transform SnappingPointTop;


    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (this.enabled == true)
        {
            //make character snap to ladder.
            PlayerScript otherAsPlayer = other.GetComponent<PlayerScript>();
            if (otherAsPlayer != null)
            {
                otherAsPlayer.NotifyLadderNearby(this);
            }
            //make character go up when holding W and down when holding S\
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.enabled == true)
        {
            //make character unsnap to ladder
            PlayerScript otherAsPlayer = other.GetComponent<PlayerScript>();
            if (otherAsPlayer != null)
            {
                otherAsPlayer.NotifyLadderExit(this);
            }
        }
    }

    public Transform GetClosestSnappingTransform(Vector3 Pos)
    {
        float DistanceToTop = Vector3.Distance(Pos, SnappingPointTop.position);
        float DistanceToBottom = Vector3.Distance(Pos, SnappingPointBottom.position);
        return DistanceToTop < DistanceToBottom ? SnappingPointTop : SnappingPointBottom;
    }
}
