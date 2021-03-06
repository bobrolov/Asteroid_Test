using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderController : MonoBehaviour
{
    private float xBorder;
    private float yBorder;

    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        xBorder = gameController.xBorder;
        yBorder = gameController.yBorder;
    }
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
