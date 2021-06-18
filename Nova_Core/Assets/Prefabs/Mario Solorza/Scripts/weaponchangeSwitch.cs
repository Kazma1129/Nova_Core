using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class weaponchangeSwitch : MonoBehaviour
{
   
    public Transform g1;
    public Transform g2;

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame) {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            GameManager.Instance.a1 = false;
            GameManager.Instance.a2 = false;
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame){
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
            GameManager.Instance.a1 = true;
            GameManager.Instance.a2 = false;
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame){
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            GameManager.Instance.a1 = false;
            GameManager.Instance.a2 = true;            
        }
    }

}

  

       
    





