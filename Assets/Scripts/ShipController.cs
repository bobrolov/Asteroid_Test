using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [Header("Параметры перемещения")]
    [SerializeField]
    private float rotationSpeed = 1f;
    [SerializeField]
    private float movementSpeed = 1f;
    [SerializeField]
    private float flameCooldown = 1f;
    private bool isFlameShow = true;
    private float flameTimer = 0;

    [Header("Параметры стрельбы")]
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float shootCooldown = 1f;
    private float shootTimer = 0f;
    private Rigidbody2D rb2D;

    [Header("Звуки")]
    [SerializeField]
    private AudioSource fireSound;
    [SerializeField]
    private AudioSource destroyShipSound;


    

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Shoot();
        Movement();
        
    }

    void Movement()
    {
        if ((Input.GetKey(KeyCode.UpArrow)) ||
            (Input.GetKey(KeyCode.W)))
        {
            rb2D.AddRelativeForce(Vector2.up * movementSpeed * Time.deltaTime);
            //мерцание огня
            if (flameTimer <= 0)
            {
                isFlameShow = ShowFlame(isFlameShow);
                flameTimer = flameCooldown;
            }
            else
                flameTimer -= Time.deltaTime;
        }
        else if (isFlameShow)
        {
            isFlameShow = ShowFlame(isFlameShow);
            flameTimer = 0;
        }

        if ((Input.GetKey(KeyCode.RightArrow)) ||
            (Input.GetKey(KeyCode.D)))
        {
            transform.Rotate(Vector3.back * rotationSpeed * Time.deltaTime);
        }

        if ((Input.GetKey(KeyCode.LeftArrow)) ||
            (Input.GetKey(KeyCode.A)))
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }

    }

    void Shoot ()
    {
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            Instantiate(bullet, transform.TransformPoint(new Vector3(0,2.5f,0)), transform.rotation);
            fireSound.Play();
            shootTimer = shootCooldown;
        }
    }

    bool ShowFlame (bool isShowNow)
    {
        transform.Find("Flame").GetComponent<SpriteRenderer>().enabled = !isShowNow;
        return !isShowNow;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Bullet")
        {
            destroyShipSound.Play();
            transform.position = Vector3.zero;
            rb2D.velocity = Vector2.zero;
            transform.rotation = Quaternion.identity;
        }
    }
}
