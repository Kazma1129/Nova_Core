using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class H2 : MonoBehaviour
{

    Rigidbody rb;
    public float multiplier;
    public float moveForce, turnTorque;
    public Transform[] anchors = new Transform[4];
    RaycastHit[] hits = new RaycastHit[4];
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            applyForce(anchors[i], hits[i]);
        }
    }
    void applyForce(Transform anchor, RaycastHit hit)
    {

        if (Physics.Raycast(anchor.position, -anchor.up, out hit))
        {
            float force = 0;
            force = Mathf.Abs(1 / (hit.point.y - anchor.position.y));
            rb.AddForceAtPosition(transform.up * force * multiplier, anchor.position, ForceMode.Acceleration);
        }

    }
}
