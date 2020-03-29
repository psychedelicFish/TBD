using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/************************************************
 *  Controller is used on the player ships
 *  Controls the firing
 ***********************************************/
public class Controller : MonoBehaviour
{
    //! Reference to the class that will instatiate the bullet
    public Weapon hardpoint;
    //! Has this controller already fired
    /*! Need to stop players from been able to spam missles */
    public bool fired = false;

    //!Start function
    private void Start()
    {
        CollisionController.onHit += ToggleFire;
    }

    //!Function that handles the firing of the Weapon class 
    /*! Checks which players turn it is then proceeds
     * to call the fire function from that players controllers 
     * Weapon reference */
    public void Fire()
    {
        if (!fired)
        {
            if(GameController.instance.turn == GameController.Turn.Player1)
            {
                Vector3 rotation = new Vector3(Mathf.Cos(GameController.instance.Angle), Mathf.Sin(GameController.instance.Angle), 0);
                //hardpoint.RotateWeapon();
                hardpoint.Fire(rotation);
                fired = true;
            }
            if (GameController.instance.turn == GameController.Turn.Player2)
            {
                Vector3 rotation = new Vector3(-(Mathf.Cos(GameController.instance.Angle)), Mathf.Sin(GameController.instance.Angle), 0);
                //hardpoint.RotateWeapon();
                hardpoint.Fire(rotation);
                fired = true;
            }

        }
    }
    //*Toggles fired to false
    void ToggleFire()
    {
        fired = false;
    }
}
