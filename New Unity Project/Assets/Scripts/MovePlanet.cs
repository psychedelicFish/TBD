using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlanet : MonoBehaviour
{


    private Rigidbody rigidbody;
    public Vector3 gravityForce;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        //
        gravityForce = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
