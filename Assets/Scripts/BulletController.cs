using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("Параметры пули")]
    [SerializeField]
    private float bulletSpeed = 500f;
    [SerializeField]
    private float bulletDestroyTime = 1.5f;

    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * bulletSpeed);
        Destroy(gameObject, bulletDestroyTime);
    }
}
