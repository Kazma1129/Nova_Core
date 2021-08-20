using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageToEnemy : MonoBehaviour
{

    public static bool pleaseDestroy = false;

    // Update is called once per frame
    void Update()
    {
        if (pleaseDestroy)
        {
            Destroy(this.gameObject, .3f);
        }
    }




}
