using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private bool hit;
    private static float damageReceived;
    public Transform PlayerTransform;
    public Transform TeleportGoal;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet Enemy") {
            hit = true;
            damageReceived = EnemyShoot.damageToPlayer;
            
        }
    }

    private void Awake()
    {
    }



    // Update is called once per frame
    void Update()
    {

        if (hit) {
            GameManager.Instance.playerHP -= damageReceived;
            hit = false;
        }

        if (GameManager.Instance.playerHP <= 0) {
            GameManager.Instance.playerHP = 100f;
            PlayerTransform.position = TeleportGoal.position;// should work only teleporting children objects for some reason.
        }

    }
}
