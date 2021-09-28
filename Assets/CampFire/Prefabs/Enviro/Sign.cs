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
        Debug.Log(currentDialogIndex);
        GoToNextDialog();
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractComponent interactComponent = other.GetComponent<InteractComponent>();
        if(interactComponent!=null)
        {
            if(TransitionCoroutine != null)
            {
                StopCoroutine(TransitionCoroutine);
                TransitionCoroutine = null;
            }
            TransitionCoroutine = StartCoroutine(TransitionOpacityTo(1));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractComponent interactComponent = other.GetComponent<InteractComponent>();
        if (interactComponent != null)
        {
            if (TransitionCoroutine != null)
            {
                StopCoroutine(TransitionCoroutine);
                TransitionCoroutine = null;
            }
            TransitionCoroutine = StartCoroutine(TransitionOpacityTo(0));
        }
    }
}
