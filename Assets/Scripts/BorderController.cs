using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderController : MonoBehaviour
{
    private float xBorder;
    private float yBorder;
    private bool isBorderCross = false;

    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        xBorder = gameController.xBorder;
        yBorder = gameController.yBorder;
    }
    void Update()
    {
        isBorderCross = false;
        if (transform.position.x > xBorder)
        {
            transform.position = new Vector3(-xBorder, transform.position.y, 0);
            isBorderCross = true;
        }
        else if (transform.position.x < -xBorder)
        {
            transform.position = new Vector3(xBorder, transform.position.y, 0);
            isBorderCross = true;
        }

        if (transform.position.y > yBorder)
        {
            transform.position = new Vector3(transform.position.x, -yBorder, 0);
            isBorderCross = true;
        }
        else if (transform.position.y < -yBorder)
        {
            transform.position = new Vector3(transform.position.x, yBorder, 0);
            isBorderCross = true;
        }
        if ((tag == "Alien" || tag == "AlienSmall") && isBorderCross)
            GetComponent<AlienController>().AlienCanEscape();
    }
}
