using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smovement : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    private CharacterController cc;
    public float speed = 12f;

    public float gravity = -9.81f;
    public Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask floorMask;
    bool isGrounded;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Transform cam;


    // Start is called before the first frame update
    void Start()
    {
        cc = gameObject.GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;



        if (direction.magnitude >= 0.1f) {
            cc.Move(direction * speed * Time.deltaTime);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            cc.Move(moveDir.normalized * speed * Time.deltaTime);

        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, floorMask);

        velocity.y += gravity * Time.deltaTime;
        if (isGrounded && velocity.y < 0) {
            velocity.y = -1.86f; 
        }

        cc.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump")) {
            velocity.y = Mathf.Sqrt(3 * -2 * gravity);
        }



    }
}
