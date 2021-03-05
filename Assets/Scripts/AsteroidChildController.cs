using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidChildController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        transform.parent.GetComponent<AsteroidController>().CollisionDetect(collision);
    }
}
