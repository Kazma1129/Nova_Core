using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBullet : MonoBehaviour
{

    

    private void Awake()
    {
       
    }


    public float destroyTime;
    // Update is called once per frame
    void Update()
    {
    

        Destroy(this.gameObject, destroyTime);
    }
}
