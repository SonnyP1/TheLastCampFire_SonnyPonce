using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : InteractableScript
{
    [SerializeField] Image DialogBG;
    [SerializeField] Text DialogText;
    [SerializeField] float TransitionSpeed = 1;
    [SerializeField] string[] dialogs;
    Color DialogTextColor;
    Color DialogBGColor;
    int currentDialogIndex = 0;
    float Opacity;
    Coroutine TransitionCoroutine;

    void GoToNextDialog()
    {
        if(dialogs.Length == 0)
        {
            return;
        }

        if(dialogs.Length == currentDialogIndex)
        {
            return;
        }
        currentDialogIndex = (currentDialogIndex + 1) % dialogs.Length;
        DialogText.text = dialogs[currentDialogIndex];
    }
    private void Start()
    {
        DialogTextColor = DialogText.color;
        DialogBGColor = DialogBG.color;
        SetOpacity(0);
        if(dialogs.Length !=0)
        {
            DialogText.text = dialogs[0];
        }
        else
        {
            DialogText.text = "";
        }
    }

    void SetOpacity(float opacity)
    {
        opacity = Mathf.Clamp(opacity, 0, 1);
        Color ColorMult = new Color(1f, 1f, 1f,opacity);
        DialogText.color = DialogTextColor *ColorMult;
        DialogBG.color = DialogBGColor * ColorMult;
        Opacity = opacity;
    }

    IEnumerator TransitionOpacityTo(float newOpacity)
    {
        float Dir = newOpacity - Opacity > 0 ? 1 : -1;
        while(Opacity != newOpacity)
        {
            SetOpacity(Opacity + Dir * TransitionSpeed*Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        SetOpacity(newOpacity);
    }
    public override void Interact()
    {
        base.Interact();
        if (currentDialogIndex != dialogs.Length - 1)
        {
            GoToNextDialog();
        }
        else
        {
            StopPreviousTransitionCoroutineStartNewOne(0);
        }
    }

    void StopPreviousTransitionCoroutineStartNewOne(float newOpacity)
    {
        if (TransitionCoroutine != null)
        {
            StopCoroutine(TransitionCoroutine);
            TransitionCoroutine = null;
        }
        TransitionCoroutine = StartCoroutine(TransitionOpacityTo(newOpacity));
    }
    private void OnTriggerEnter(Collider other)
    {
        currentDialogIndex = 0;
        DialogText.text = dialogs[currentDialogIndex];
        InteractComponent interactComponent = other.GetComponent<InteractComponent>();
        if(interactComponent!=null)
        {
            StopPreviousTransitionCoroutineStartNewOne(1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractComponent interactComponent = other.GetComponent<InteractComponent>();
        if (interactComponent != null)
        {
            StopPreviousTransitionCoroutineStartNewOne(0);
        }
    }
}
