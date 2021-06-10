using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook1 : MonoBehaviour
{
    [Header("Mouse Velocity")]
    public float mouseVelocityX;
    public float mouseVelocityY;

    public Transform playerBody;

    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
       float mouseX = Input.GetAxis("Mouse X") * mouseVelocityX * Time.deltaTime;
       float mouseY = Input.GetAxis("Mouse Y") * mouseVelocityY * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX); // updates player body to rotate camera horizontally
        xRotation -= mouseY; //xRotation += mouseY; for inverted controller
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

    }
}
