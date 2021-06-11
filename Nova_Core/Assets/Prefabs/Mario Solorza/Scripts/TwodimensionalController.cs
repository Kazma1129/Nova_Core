using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwodimensionalController : MonoBehaviour
{
    [Header("Movement variables")]
    private CharacterController controller;
    public float speed;
    Vector3 velocity;
    public float jumpHeight;
    public float gravity = -9.81f;
    public bool isGrounded;
    public LayerMask groundMask;
    public Transform groundCheck;
    public float radiusCheck;



    [Header ("Animation variables")]
    Animator animator;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    public float acceleration = 2.0f;
    public float deceleration = 0.5f;
    public float maxWalkVelocity = 0.5f;
    public float maximumRunVelocity = 2.0f;
    //increase performance less proan to error while typing the animator reference
    int VelocityZHash;
    int VelocityXHash;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        VelocityZHash = Animator.StringToHash("Velocity Z");
        VelocityXHash = Animator.StringToHash("Velocity X");
        Cursor.lockState = CursorLockMode.Locked;
    }

    // increases or decreases velocity of animation based on keypresses.
    void changeVelocity(bool forwardPressed, bool backPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity) {
        ///////////////////ACCELERATION ///////////////////////////////////////

        // if forward is pressed increase velocity on z;
        if (forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        // if back key is pressed  decrease z velocity

        if (backPressed && !runPressed && velocityZ > -currentMaxVelocity)
        {

            velocityZ -= Time.deltaTime * acceleration;
        }

        //increase velocity left direction
        if (leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        //increase velocity right direction
        if (rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        //////////////////////// DECELERATION //////////////////////////////

        //decelerates if forward is not pressed and velocity is over 0
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        //decelerates if back is not pressed and velocity is over 0
        if (!backPressed && velocityZ < 0.0f)
        {
            velocityZ += Time.deltaTime * deceleration;
        }

        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }

        //decelerates if back is not pressed and velocity is over 0
        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }


    }
    //locks and reset velocity of animation based on thresholds.
    void lockOrResetVelocity(bool forwardPressed, bool backPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity) {

        //reset speed if keys are not pressed so it can go into the idle animation  needs fixing to transition smoothly once buttons are let go
        if (!leftPressed && !rightPressed && velocityX != 0 && (velocityX > -currentMaxVelocity && velocityX < currentMaxVelocity))
        { // for left and right
            velocityX = 0.0f;
        }
        if (!forwardPressed && !backPressed && velocityZ != 0 && (velocityZ > -currentMaxVelocity && velocityZ < currentMaxVelocity)) // for up and down
        {
            velocityZ = 0.0f;
        }
        /////////////////////////////////////////////////////////////////////


        //Lock excess velocity when running forward
        if (forwardPressed && runPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        //decelerates to the maximum walk velocity --added instead of resseting the speed since that messed up the animations.
        else if (forwardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;

            //round to the current max velocity if within offset
            if (velocityZ > currentMaxVelocity && velocityZ < (currentMaxVelocity + 0.05f))
            {
                velocityZ = currentMaxVelocity;
            }

        }
        //round to the current max velocity if within offset
        else if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f))
        {
            velocityZ = currentMaxVelocity;
        }
        ///////////////////////////////////////////////////////////


        //Lock excess velocity when going back
        if (backPressed && runPressed && velocityZ < -currentMaxVelocity)
        {
            velocityZ = -currentMaxVelocity;
        }
        //decelerates to the maximum walk velocity --added instead of resseting the speed since that messed up the animations.
        else if (backPressed && velocityZ < -currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * deceleration;

            //round to the current max velocity if within offset
            if (velocityZ < -currentMaxVelocity && velocityZ > (-currentMaxVelocity - 0.05f))
            {
                velocityZ = -currentMaxVelocity;
            }

        }

        //round to the current max velocity if within offset
        else if (backPressed && velocityZ > -currentMaxVelocity && velocityZ < (-currentMaxVelocity + 0.05f))
        {
            velocityZ = -currentMaxVelocity;
        }
        ////////////////////////////////////////////////////////////////


        //Lock excess velocity when running right
        if (rightPressed && runPressed && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        //decelerates to the maximum walk velocity --added instead of resseting the speed since that messed up the animations.
        else if (rightPressed && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;

            //round to the current max velocity if within offset
            if (velocityX > currentMaxVelocity && velocityX < (currentMaxVelocity + 0.05f))
            {
                velocityX = currentMaxVelocity;
            }

        }
        //round to the current max velocity if within offset
        else if (rightPressed && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity - 0.05f))
        {
            velocityX = currentMaxVelocity;
        }
        ////////////////////////////////////////////////////////////////

        //Lock excess velocity when running right
        if (leftPressed && runPressed && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        //decelerates to the maximum walk velocity --added instead of resseting the speed since that messed up the animations.
        else if (leftPressed && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * deceleration;

            //round to the current max velocity if within offset
            if (velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity - 0.05f))
            {
                velocityX = -currentMaxVelocity;
            }

        }
        //round to the current max velocity if within offset
        else if (leftPressed && velocityX > -currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.05f))
        {
            velocityX = -currentMaxVelocity;
        }
        ////////////////////////////////////////////////////////////////

    }

    void moveCharacter() {

        isGrounded = Physics.CheckSphere(groundCheck.position, radiusCheck, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        /*
        velocityX = Input.GetAxis("Horizontal");
        velocityZ = Input.GetAxis("Vertical");
        */

        Vector3 move = transform.right * velocityX + transform.forward * velocityZ;

        controller.Move(move * Time.deltaTime * speed);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);






        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //input reading for animation
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool backPressed = Input.GetKey(KeyCode.S);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);


        moveCharacter();
        //set current MaxVelocity
        float currentMaxVelocity = runPressed ? maximumRunVelocity : maxWalkVelocity; // ternaty operator for max velocity == if run is pressed, set the maximum velocity value if not set max walk value.
        changeVelocity(forwardPressed,backPressed,leftPressed,rightPressed,runPressed,currentMaxVelocity);
        lockOrResetVelocity(forwardPressed, backPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);

        // set the parameters to out local variable values.
        animator.SetFloat(VelocityZHash, velocityZ);
        animator.SetFloat(VelocityXHash, velocityX);

    }

}
