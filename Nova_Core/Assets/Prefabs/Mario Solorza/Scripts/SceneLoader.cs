using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void sceneStart() {
        SceneManager.LoadScene(1);
    }

    public void mainScreen() {
        SceneManager.LoadScene(0);
    }

    public void salir() {
        Application.Quit();
    }

    public void credits()
    {
        SceneManager.LoadScene("Credits");
    }

 

}
