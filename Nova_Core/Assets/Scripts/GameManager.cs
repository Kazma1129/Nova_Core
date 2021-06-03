using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public int coins;
    public int hp;
    public int potions;

    public int keysNeeded = 3;
    public bool[] keys;

    public bool a1;
    public bool a2;
    public bool a3;
    public bool a4;

    public int b1 = 0;
    public int b2;
    public int b3;
    public int b4;

    public bool lava = false;

    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
    }

    /*

    private void Update()
    {
        if (b1 >= 300) {
            b1 = 300;
        }
        if (b2 >= 200)
        {
            b2 = 200;
        }
        if (b3 >= 200)
        {
            b3 = 200;
        }
        if (b4 >= 100)
        {
            b4 = 100;
        }
    }*/
}
