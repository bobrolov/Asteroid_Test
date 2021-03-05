using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderController : MonoBehaviour
{
    [SerializeField]
    private float xBorder = 44f;
    [SerializeField]
    private float yBorder = 24f;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > xBorder)
            transform.position = new Vector3(-xBorder, transform.position.y, 0);
        else if (transform.position.x < -xBorder)
            transform.position = new Vector3(xBorder, transform.position.y, 0);

        if (transform.position.y > yBorder)
            transform.position = new Vector3(transform.position.x, -yBorder , 0);
        else if (transform.position.y < -yBorder)
            transform.position = new Vector3(transform.position.x, yBorder, 0);
    }
}
