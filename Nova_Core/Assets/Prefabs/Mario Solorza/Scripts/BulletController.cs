using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletDecal;

    public float speed = 50f;
    public float timeToDestroy = 3f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }


    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //destroys game object if is close enough of the target // or if there's no target.
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (!hit && Vector3.Distance(transform.position, target) < .01f) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);     //offsetting this by .0001f so it renders correctly when hitting the wall.
        GameObject.Instantiate(bulletDecal, contact.point + contact.normal*.0001f, Quaternion.LookRotation(contact.normal)); // instantiates object on collission in wall, floor, etc.
        Destroy(gameObject);
    }

}
