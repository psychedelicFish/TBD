using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Weapon hardpoint;
    public bool fired = false;

    private void Start()
    {
        CollisionController.onHit += ToggleFire;
    }

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
    void ToggleFire()
    {
        fired = false;
    }
}
