using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotNumerics;
using System;


/***********************************************************
 *  Gravity is my own class to handle how gravity works 
 *  within the game. Each of the planets is considered a 
 *  affector. They each apply a force to the missle. This 
 *  class adds those forces up and applys it to the missle
 **********************************************************/
public class Gravity : MonoBehaviour
{
    
    //! float for how big gravity is 
    float gravity;
    
    //! adds an intial velocity to the object
    public float intialVelocity;
    
    //! is the object affected by gravity
    public bool move;
    
    //! List of affectors
    /*! This gets added to within OnTriggerStay
     * but only if the object in question isnt already in there*/
    public List<Rigidbody> affectors = new List<Rigidbody>();

    //! Reference to the objects rigidBody
    /*! Set within awake */
    Rigidbody source;
    //! Vector 2 to store the calculated acceleration
    Vector2 acceleration;
    //! stores the objects intial velocity
    Vector2 velocity = new Vector3(0, 0, 0);

    //!Awake function - Intialisation done in here
    private void Awake()
    {
        gravity = 1f; // make this bigger
        source = gameObject.GetComponent<Rigidbody>();
        velocity = gameObject.transform.forward * intialVelocity;
    }

    //!OnTriggerStay - Unity function takes a Collider as a parameter
    /*! This function is used to track which planets are acting on the missile
     * This information is then used within fixed update to affect the velocity
     * of the missle */
    void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.CompareTag("Affector"))
        {
            return;
        }
        Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();
        if (affectors.Contains(otherRigidbody))
        {
            return;
        }
        affectors.Add(otherRigidbody);
        
    }
    //! Fixed update - Handles all the maths
    /*! Within this function the magic of gravity happens
     * based on the list of affectors a new velocity is created for the 
     * missle. This is done using leapfrog integration */
    private void FixedUpdate()
    {
        //Process the objects in the list
        foreach(var body in affectors)
        {
            Vector2 gravityDirection = gameObject.transform.position - body.gameObject.transform.position;

            float r = gravityDirection.magnitude;

            var rnorm = gravityDirection.normalized;
            Debug.Log(r);

            var gravityForce = -(((gravity * source.mass * body.mass) / (r * r)) * rnorm);
            Debug.DrawLine(transform.position, body.gameObject.transform.position);
            acceleration += gravityForce;
        }
        affectors.Clear();
        if (move)
        {
            velocity += acceleration * 0.5f * Time.fixedDeltaTime;
            source.position += (Vector3)velocity * Time.fixedDeltaTime;
            velocity += acceleration * 0.5f * Time.fixedDeltaTime;
        }
    }
}
