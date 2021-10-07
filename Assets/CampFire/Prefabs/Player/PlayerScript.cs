using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] MovementComp movementComp;
    [SerializeField] LadderMovement ladderComp;

    [Header("Other")]

    [SerializeField] GameObject Owner;

    PlayerInputs playerInputs;

    public PlayerInputs GetPlayerInput() { return playerInputs; }
    private void Awake()
    {
        playerInputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }
    private void OnDisable()
    {
        playerInputs.Disable();
    }
    
    private void Start()
    {
        movementComp = GetComponent<MovementComp>();
        ladderComp = GetComponent<LadderMovement>();
        ladderComp.SetInput(playerInputs);
        playerInputs.Gameplay.Move.performed += OnMoveInputUpdated;
        playerInputs.Gameplay.Move.canceled += OnMoveInputUpdated;
        playerInputs.Gameplay.Interact.performed += OnInteractPressed;
    }

    void OnInteractPressed(InputAction.CallbackContext ctx)
    {
        InteractComponent interactComp = GetComponentInChildren<InteractComponent>();
        if(interactComp!=null)
        {
            interactComp.Interact(Owner);
        }
    }
    void OnMoveInputUpdated(InputAction.CallbackContext ctx)
    {
        movementComp.SetMovementInput(ctx.ReadValue<Vector2>());
    }



    public void EnabledMovementInput()
    {
        playerInputs.Gameplay.Move.Enable();
    }
    public void DisableMovementInput()
    {
        playerInputs.Gameplay.Move.Disable();
    }
}
