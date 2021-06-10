using UnityEngine;


public class WepRaycast : MonoBehaviour
{
    public GameObject wepParent;

    public Transform spawnPoint;

    public float cooldown;
    private float timer;
    public float blife;

    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public float impactForce = 30f;
    public float fireRate = 15f;
    public float nextTimeToFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            shoot();
        }
    }


    void shoot()
    {
      //  AudioManager.Instance.Play("Shoot");
      //  muzzleFlash.Play(); // particle animation effect
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)){
            //Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range);
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null) {
                target.takeDamage(damage);
            }
        }

        if (hit.rigidbody != null) {
            hit.rigidbody.AddForce(hit.normal * (-impactForce));
        }
        GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impactGO, 2f);



    }

}
