using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonDash : MonoBehaviour
{
    CharacterController moveScript;
    private CharacterController controller; //character controller
    public float dashSpeed;
    public float dashtime;
    private PlayerInput playerInput;
    private InputAction dashAction;
    private bool groundedPlayer;
    private Transform cameraTransform; //to get main camera.

    // Start is called before the first frame update
    void Awake()
    {
        cameraTransform = Camera.main.transform;

        moveScript = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {

        

    }
    /*
    private void OnEnable()
    {

       // dashAction.performed += _ => Dash();

    }

    private void OnDisable()
    {
       // dashAction.performed -= _ => Dash();
    }
    */
    private void Dash() {
        Vector2 input = dashAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);

        // takes into account camera direction while moving.
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        //to avoid unity setting the y value itself
        move.y = 0;
        controller.Move(move * Time.deltaTime * dashSpeed);
        
    }
}
