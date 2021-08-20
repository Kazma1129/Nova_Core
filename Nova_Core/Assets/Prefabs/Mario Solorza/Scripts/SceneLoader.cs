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
        SceneManager.LoadScene("Creditos");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            Ending();
        }
    }

    public void Ending()
    {
        SceneManager.LoadScene("Ending");
    }




}
