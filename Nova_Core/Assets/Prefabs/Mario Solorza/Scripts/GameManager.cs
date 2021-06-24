using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [Range(0.0f, 100.0f)]
    public float playerHP = 100f;

    public bool a1 = false;
    public bool a2 = false;


    public int ammo2 = 100;
    public float bullet1Damage = 10;







    void Awake()
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


}
