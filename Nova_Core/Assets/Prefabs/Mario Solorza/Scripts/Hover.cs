using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hover : MonoBehaviour
{
    Rigidbody rb;
    public float multiplier;
    public float moveForce, turnTorque;
    public Transform[] anchors = new Transform[4];
    RaycastHit[] hits = new RaycastHit[4];
    private PlayerInput playerInput; // to get input actions from the input system
    private InputAction hoverAction;
    public Transform hoverObject;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        hoverAction = playerInput.actions["Hover"];
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void FixedUpdate()
    {
        if (hoverAction.triggered)
        {
            hoverObject.gameObject.SetActive(true);
            for (int i = 0; i < 4; i++) {
                applyForce(anchors[i], hits[i]);
            }
        }
        else {
            hoverObject.gameObject.SetActive(false);
        }

    }
    /*
    private void OnEnable()
    {

        hoverAction.performed += _ => applyForce();

    }

    private void OnDisable()
    {
        hoverAction.performed -= _ => applyForce();
    }
    */
    // Update is called once per frame
    void Update()
    {

    }

    void applyForce(Transform anchor, RaycastHit hit) {

        if (Physics.Raycast(anchor.position, -anchor.up, out hit)) {
            float force = 0;
            force = Mathf.Abs(1 / (hit.point.y - anchor.position.y));
            rb.AddForceAtPosition(transform.up * force * multiplier, anchor.position, ForceMode.Acceleration);
        }

    }
}
