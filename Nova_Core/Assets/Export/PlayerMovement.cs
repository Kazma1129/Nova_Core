using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController controller;
    public float speed;
    Vector3 velocity;
    public float jumpHeight;
    public float gravity = -9.81f;
    public bool isGrounded;
    public LayerMask groundMask;
    public Transform groundCheck;
    public float radiusCheck;
    public float runningSpeedMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, radiusCheck,groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        
            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * Time.deltaTime * speed);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
    

        



        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

    }
}
