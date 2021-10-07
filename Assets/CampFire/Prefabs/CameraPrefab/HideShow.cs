using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideShow : MonoBehaviour
{
    [SerializeField] Text txt;
    void Start()
    {
        txt.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            txt.enabled = true;
        }
    }
}
