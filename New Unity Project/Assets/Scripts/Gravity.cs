using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotNumerics;
using System;

public class Gravity : MonoBehaviour
{
    float gravity;
    public float intialVelocity;
    public bool move;
    public List<Rigidbody> affectors = new List<Rigidbody>();

    Rigidbody source;
    Vector2 acceleration;
    Vector2 velocity = new Vector3(0, 0, 0);
    Vector3 orbitalVelocity;


    private void Awake()
    {
        gravity = 1f; // make this bigger
        source = gameObject.GetComponent<Rigidbody>();
        velocity = gameObject.transform.forward * intialVelocity;
    }

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
