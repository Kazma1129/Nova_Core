using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class SwitchVCam : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private int priorityBoosAmount = 10;
    [SerializeField]
    private Canvas thirdPersonCanvas;
    [SerializeField]
    private Canvas AimCanvas;

    private CinemachineVirtualCamera virtualCamera;
    private InputAction aimAction;
    
    // Start is called before the first frame update

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimAction = playerInput.actions["Aim"];
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }

    private void StartAim() {
        virtualCamera.Priority += priorityBoosAmount;
        AimCanvas.enabled = true;
        thirdPersonCanvas.enabled = false;
    }

    private void CancelAim() {
        virtualCamera.Priority -= priorityBoosAmount;
        AimCanvas.enabled = false;
        thirdPersonCanvas.enabled = true;
    }
}
