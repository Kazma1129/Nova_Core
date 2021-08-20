using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private bool hit;
    private static float damageReceived;
    public Transform PlayerTransform;
    public GameObject TeleportGoal;
    [SerializeField]
    public CharacterController chara;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet Enemy") {
            hit = true;
            damageReceived = EnemyAi2.damageToPlayer;
            
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (hit)
        {
            GameManager.Instance.playerHP -= damageReceived;
            hit = false;
        }


        if (GameManager.Instance.playerHP <= 0)
        {
            GameManager.Instance.playerHP = 100f;
          //  GetComponent<CharacterController>().enabled = false;
            transform.position = TeleportGoal.transform.position;
            //GetComponent<CharacterController>().enabled = true;
        }
    }




}
