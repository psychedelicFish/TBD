using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***********************************************************
 *  Handles where the missle spawns from
 **********************************************************/
public class Weapon : MonoBehaviour
{
    //! Reference to the object to be instaitiated
    public GameObject bullet;

    //! Handles the instaiation of the missle, Takes a Vector3 as a parameter
    /*! This passed in Vector is used to calculate the direction the bullet will travel */
    public void Fire(Vector3 v)
    {
        Quaternion rotate = Quaternion.identity;
        rotate.SetLookRotation(v);
        var b = Instantiate(bullet, gameObject.transform.position, rotate);
        b.GetComponent<Rigidbody>().AddForce(v * GameController.instance.Power, ForceMode.Impulse);
    }
   
}

