using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    public LineRenderer path;
    public Material mat;
    Coroutine draw = null;

    private void Start()
    {
        draw = StartCoroutine(DrawPath());
    }

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

    private void OnDisable()
    {
        StopCoroutine(draw);
    }

}
