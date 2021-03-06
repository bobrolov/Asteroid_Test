using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed = 500f;
    [SerializeField]
    private float bulletDestroyTime = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);
        Destroy(gameObject, bulletDestroyTime);
    }
}
