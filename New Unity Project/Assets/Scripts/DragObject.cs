using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragObject : MonoBehaviour
{

    bool dragging;
    float distance;

    public bool moveOnX;
    public bool moveOnY;
    // Start is called before the first frame update
    void Start()
    {
        dragging = false;
    }

    private void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        dragging = true;
    }

    private void OnMouseUp()
    {
        dragging = false;
    }

    // Update is called once per frame
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
