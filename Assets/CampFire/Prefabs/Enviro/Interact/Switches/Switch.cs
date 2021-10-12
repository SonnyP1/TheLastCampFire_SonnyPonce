using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : InteractableScript
{
    bool isSwitchOn = false;
    public override void Interact()
    {
        if (!isSwitchOn)
        {
            SwitchOn();
        }
        else
        {
            SwitchOff();
        }
    }


    public virtual void SwitchOn()
    {
        isSwitchOn = true;
    }

    public virtual void SwitchOff()
    {
        isSwitchOn = false;
    }
}
