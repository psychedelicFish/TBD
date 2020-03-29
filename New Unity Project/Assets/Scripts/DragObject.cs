using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***********************************************************
 *  Drag object handles the dragging of planets and players
 *  within the setup stage of the game
 **********************************************************/
public class DragObject : MonoBehaviour
{

    //! Bool to check if player is dragging an object
    bool dragging;
    //! how far is the object been dragged
    float distance;

    //!Bool - Can the object be moved on X axis
    public bool moveOnX;
    //!Bool - Can the object be moved on Y axis
    public bool moveOnY;
    //! Start function
    void Start()
    {
        dragging = false;
    }

    //! Controls what happens when mouse button is pressed
    private void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        dragging = true;
    }

    //!Controls what happens when the mouse is released
    private void OnMouseUp()
    {
        dragging = false;
    }

    
    //!Update - Where all the maths happens for dragging 
    void Update()
    {
        if (dragging && GameController.instance.gameState == GameController.GameState.SETUP)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            var t = transform.position;

            if (moveOnY)
            {
                t.y = rayPoint.y;
            }
            if (moveOnX)
            {
                t.x = rayPoint.x;
            }
            
            transform.position = t;
        }
    }
}
