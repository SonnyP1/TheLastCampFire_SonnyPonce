using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    PlayerInputs playerInputs;
    Vector2 MoveInput;
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
        playerInputs.Gameplay.Move.performed += OnMoveInputUpdated;
        playerInputs.Gameplay.Move.canceled += OnMoveInputUpdated;
    }
    void OnMoveInputUpdated(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        Debug.Log($"Player Input is: {MoveInput}");
    }
}
