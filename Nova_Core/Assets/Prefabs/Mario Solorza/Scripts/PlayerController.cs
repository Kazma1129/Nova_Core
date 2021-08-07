using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections; //*****Lo agregue para el mi IEnumerator


[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float DoublejumpHeight = 1.0f;
    [SerializeField]
    //*****No se porque pones el [SerializeField] pero segui tu orden
    private bool doubleJump;
    [SerializeField]
    private bool isJumping = false;
    [SerializeField]
    private float jumpCooldown;
    [SerializeField]
    private float jumpTimer;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 25f;
    [SerializeField]
    private GameObject bulletPrefab; // bullet prefab
    [SerializeField]
    private Transform barrelTransform; // bullet spawn point
    [SerializeField]
    private float dashSpeed = 25f;
    //[SerializeField]
    //private float dashTime = .5f; tried to use this for dash time
    // [SerializeField]  can be used for projectiles, but we are raycasting.
    // private Transform bulletParent;
    [SerializeField]
    private float bulletHitMissDistance = 25f;
    [SerializeField]
    private float hoverTime = 2f;
    public float hoverTimer = 0;
    public bool isHovering = false;


    private CharacterController controller; //character controller
    private PlayerInput playerInput; // to get input actions from the input system
    private Vector3 playerVelocity;
    public bool groundedPlayer;
    private Transform cameraTransform; //to get main camera.

    //Declaring input actions from input system.
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;
    private InputAction dashAction;
    private InputAction hoverAction;

    /// <summary>
    /// Animation variables
    /// </summary>

    public bool isRunning = false;
    public bool idleLong = false;
    public float idleTimer = 3f;
    private float idleTime = 0f;
    private bool isShooting = false;
    private bool moveHash;
    Animator animator;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        animator = GetComponent<Animator>();

        // setting up input values from new input system and cursos lock state so we are bound to the screen.
        moveAction = playerInput.actions["Movement"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        dashAction = playerInput.actions["Dash"];
        hoverAction = playerInput.actions["Hover"];

        Cursor.lockState = CursorLockMode.Locked;
    }

    //Shoots
    private void OnEnable()
    {
        hoverAction.performed += _ => hover();// subscribes to hover event.
        shootAction.performed += _ => shootGun(); // subscribes to shooting event.
        
    }

    private void OnDisable()
    {
        hoverAction.performed -= _ => hover(); // de-subscribes to hover event.
        shootAction.performed -= _ => shootGun();  // de-subscribes to shooting event.

    }

    // hover function // see code below for how this was disabled.
    void hover() {
        hoverTimer = 0;
        gravityValue = 0;
        isHovering = true;
    }

    void Stophover()
    {
        gravityValue = -9.81f;
    }

    void runningAction() {
        isRunning = true;
    }
    void stopRunningAction()
    {
        isRunning = false;
    }



    private void shootGun()
    {
        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity /*, bulletParent*/);
        BulletController bulletController = bullet.GetComponent<BulletController>();
    
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity)) //basically if raycast hits
        {

            bulletController.target = hit.point; //targets equals hit point
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
            bulletController.hit = false;
        }
        

    }
    //shoot end

    //*****Esto es para poder meterlo al main, es el cooldown del salto
    IEnumerator waiter()
    {
        yield return new WaitForSeconds(2);
        hoverAction.Disable();

    }





    void Update()
    {
        jumpTimer -= Time.deltaTime;
        hoverTimer += Time.deltaTime;
        idleTimer += Time.deltaTime;

        
        
        if (playerInput.actions["Movement"].IsPressed()) {
            runningAction();
        }
        else if (!playerInput.actions["Movement"].IsPressed())
        {
           stopRunningAction();
        }
        animator.SetBool("isRunning", isRunning);




        if (idleTime > idleTimer && !isRunning && !isShooting)
        {
            idleLong = true;
        }
        else {
            idleLong = false;
        }


        if (GameManager.Instance.a1 == false)
        {
            shootAction.Disable();
        }
        else
        {
            shootAction.Enable();
        }

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0) //player velocity if grounded //to avoid going below zero
        {
            playerVelocity.y = 0f;
        }

        
        
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        

        // takes into account camera direction while moving.
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        //to avoid unity setting the y value itself
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed); //moves
      

        //handles hover.
        if (hoverAction.triggered && !isHovering)
        {
            
            hoverAction.Enable();
        }

        if (isHovering && hoverTimer > hoverTime)
        {
            isHovering = false;
            Stophover(); // call the function directly for some reason doing this from DISABLE did not worked.
        }



        // Changes the height position of the player...so a jump
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            isJumping = true;
            animator.SetBool("isJumping", isJumping);
            jumpTimer = jumpCooldown;
        }

        if (jumpAction.triggered && doubleJump == true && isJumping == true && jumpTimer < 0)
        {
            playerVelocity.y += Mathf.Sqrt(DoublejumpHeight * -3.0f * gravityValue);
            isJumping = false;
          
        }
        if (/*!groundedPlayer &&*/ dashAction.triggered) // dash remove commented code to enable only on air or ground dash
        {
            /* float startTime = Time.time;
             while(Time.time < startTime + dashTime) //tried adding dash time to the player, turn out doing this hangs the project.
             { */
            controller.Move(move * Time.deltaTime * playerSpeed * dashSpeed);
            /*   return;
            }*/
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
