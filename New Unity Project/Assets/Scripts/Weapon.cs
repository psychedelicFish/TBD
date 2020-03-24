using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    Transform parent;
    public GameObject bullet;
    Vector2 fireDirection;

    private void Start()
    {
        parent = gameObject.transform.parent;
        fireDirection = gameObject.transform.position - parent.position;
        fireDirection.Normalize();
    }
    public void Fire(Vector3 v)
    {
        Quaternion rotate = Quaternion.identity;
        rotate.SetLookRotation(v);
        var b = Instantiate(bullet, gameObject.transform.position, rotate);
        b.GetComponent<Rigidbody>().AddForce(v * GameController.instance.Power, ForceMode.Impulse);
    }
   
}

