﻿using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 25f;
    [SerializeField]
    private GameObject bulletPrefab; // bullet prefab
    [SerializeField]
    private Transform barrelTransform; // bullet spawn point
    [SerializeField]
    private Transform bulletParent;
    [SerializeField]
    private float bulletHitMissDistance = 25f;


    private CharacterController controller; //character controller
    private PlayerInput playerInput; // to get input actions from the input system
    private Vector3 playerVelocity; 
    private bool groundedPlayer;
    private Transform cameraTransform; //to get main camera.


    //Declaring input actions from input system.
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;

 

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        // setting up input values from new input system and cursos lock state so we are bound to the screen.
        moveAction = playerInput.actions["Movement"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        
        Cursor.lockState = CursorLockMode.Locked;
    }

    //Shoots
    private void OnEnable()
    {
        shootAction.performed += _ => shootGun();
    }

    private void OnDisable()
    {
        shootAction.performed -= _ =>  shootGun();
    }

    private void shootGun()
    {
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletParent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {

            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else {
            bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
            bulletController.hit = false;
        }

    }
    //shoot end




    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        // takes into account camera direction while moving.
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        //to avoid unity setting the y value itself
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);

  

        // Changes the height position of the player...so a jump
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }


        playerVelocity.y += gravityValue * Time.deltaTime;
        //moves character.
        controller.Move(playerVelocity * Time.deltaTime);


        //rotates towards camera direction
        float targetAngle = cameraTransform.eulerAngles.y;
        Quaternion targetrotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetrotation, rotationSpeed * Time.deltaTime);
    }
}