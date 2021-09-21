using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] float WalkingSpeed = 5f;
    [SerializeField] float RotSpeed = 5f;
    [Header("Other")]
    [SerializeField] Transform GroundCheck;
    [SerializeField] float GroundCheckRadius = .1f;
    [SerializeField] LayerMask GroundLayerMask;


    CharacterController characterController;
    PlayerInputs playerInputs;
    Vector2 MoveInput;
    Vector3 Velocity;
    const float gravity = -9.8f;


    bool IsOnGround()
    {
        return Physics.CheckSphere(GroundCheck.position, GroundCheckRadius, GroundLayerMask);
    }
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
        characterController = GetComponent<CharacterController>();
        playerInputs.Gameplay.Move.performed += OnMoveInputUpdated;
        playerInputs.Gameplay.Move.canceled += OnMoveInputUpdated;
    }
    void OnMoveInputUpdated(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
    }

    private void Update()
    {
        if(IsOnGround())
        {
            Velocity.y = -0.2f;
        }
        Velocity.x = GetPlayerDesiredMoveDir().x * WalkingSpeed;
        Velocity.z = GetPlayerDesiredMoveDir().z * WalkingSpeed;
        Velocity.y += gravity * Time.deltaTime;



        characterController.Move(Velocity*Time.deltaTime);
        UpdateRotation();
        Debug.Log(Velocity);
    }

    Vector3 GetPlayerDesiredMoveDir()
    {
        return new Vector3(-MoveInput.y, 0, MoveInput.x).normalized;
    }

    void UpdateRotation()
    {
        Vector3 PlayerDesiredDir = GetPlayerDesiredMoveDir();
        if(PlayerDesiredDir.magnitude ==0)
        {
            PlayerDesiredDir = transform.forward;
        }
        Quaternion DesiredRotation = Quaternion.LookRotation(PlayerDesiredDir,Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation,DesiredRotation,Time.deltaTime * RotSpeed);
    }
}
