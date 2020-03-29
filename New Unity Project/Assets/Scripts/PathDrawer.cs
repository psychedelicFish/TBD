using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***********************************************************
 *  Pathdrawer handles the drawing of the lines that follow
 *  the missle
 **********************************************************/
public class PathDrawer : MonoBehaviour
{

    //! public reference to the missles line renderer
    public LineRenderer path;
    //! Material for line color
    public Material mat;

    //! Coroutine reference for drawing the line
    /*! needed so the coroutine can later be stopped
     * significant preformanmce drop if this 
     * isnt done */
    Coroutine draw = null;


    //! Start Function - Starts the DrawPath coroutine
    private void Start()
    {
        draw = StartCoroutine(DrawPath());
    }

    //! Coroutine for drawing a line using the referenced line renderer
    IEnumerator DrawPath()
    {
        int index = 0;
        path.positionCount = 1;
        path.SetPosition(index, gameObject.transform.position);
        path.material = mat;
        index++;
        while (gameObject.activeSelf)
        {
            path.positionCount++;
            path.SetPosition(index, gameObject.transform.position);
            index++;
            yield return new WaitForSeconds(.1f);
        }
    }

    //! On Disable
    /*! This function is called when this class is turned off 
     * it stops the draw coroutine */
    private void OnDisable()
    {
        StopCoroutine(draw);
    }

}
