using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int velocityHash;

    bool isWalking;
    bool forwardPressed;
    bool runPressed;

    float velocity = 0.0f;
    public float acceleration = 0.1f;
    public float deceleration = 0.5f;
    public float maxSpeed = 5.0f;
    public float minSpeed = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
        isWalkingHash = Animator.StringToHash("isWalking"); // passes a numerical value, this way we avoid using strings to pass animation values
        isRunningHash = Animator.StringToHash("isRunning"); // passes a numerical value, this way we avoid using strings to pass animation values
    }

    // Update is called once per frame
    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);   //passing a variable with the int hash instead of a string 
        bool isWalking = animator.GetBool(isWalkingHash);   //passing a variable with the int hash instead of a string 
        bool forwardPressed = Input.GetKey("w");            // simple check to see if player is pushing w key / for walking
        bool runPressed = Input.GetKey("left shift");       // simple check to see if player is pushing shift key / for running


        //ACCELERATION VALUES
        if (forwardPressed && velocity < maxSpeed) {

            velocity += Time.deltaTime * acceleration; //increases speed over time to go from walking to running animation.
        }
        if (!forwardPressed && velocity > minSpeed) {
            velocity -= Time.deltaTime * deceleration; //decreases speed over time if player is not pressing forward
        }



        
        if (!forwardPressed && velocity < 0.0f) {   //since speed keeps decreasing over time as per the function above, this section caps the speed drop to 0.0
            velocity = 0.0f;
        }
        
        animator.SetFloat(velocityHash,velocity);



        /*
        //if player presses w key
        if (!isWalking && forwardPressed) {
            animator.SetBool(isWalkingHash, true);
        }

        //if player is not pressing w key
        if (isWalking && !forwardPressed) {
            animator.SetBool(isWalkingHash, false);
        }


        //if player is walking and not running and presses left shift
        if (!isRunning && forwardPressed && runPressed) {
            animator.SetBool("isRunning", true);
        }

        // if player is running and stoprs running or stops walking
        if (isRunning && !forwardPressed || !runPressed) {
            animator.SetBool("isRunning", false);
        }
        */
    }
}
