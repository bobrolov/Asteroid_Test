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
    private bool isFlameShow = false;
    private float flameTimer = 0;
    private bool isFlameSoundPlay = false;

    [Header("Параметры стрельбы")]
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float shootCooldown = 1f;
    [SerializeField]
    private int bulletCounterMax = 4;
    private float shootTimer = 0f;

    [SerializeField]
    private GameObject particlesDestroy;

    private GameController gameController;


    

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
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
            GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * movementSpeed * Time.deltaTime);
            if (!isFlameSoundPlay)
            {
                transform.Find("Flame").GetComponent<AudioSource>().Play();
                isFlameSoundPlay = true;
            }
            //мерцание огня
            if (flameTimer > 0)
                flameTimer -= Time.deltaTime;
            else
            {
                isFlameShow = ShowFlame(!isFlameShow);
                flameTimer = flameCooldown;
            }
        }
        else
        {
            if (isFlameShow)
            {
                isFlameShow = ShowFlame(!isFlameShow);
                flameTimer = 0;
            }
            if (isFlameSoundPlay)
            {
                transform.Find("Flame").GetComponent<AudioSource>().Stop();
                isFlameSoundPlay = false;
            }

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
        else if (Input.GetKey(KeyCode.Space) && (GameObject.FindWithTag("BulletsShip").transform.childCount < (bulletCounterMax)))
        {
            Instantiate(bullet, transform.TransformPoint(new Vector3(0,2.5f,0)), transform.rotation, GameObject.FindWithTag("BulletsShip").transform);
            GetComponent<AudioSource>().Play();
            shootTimer = shootCooldown;
        }
    }

    bool ShowFlame (bool isNeedToShow)
    {
        transform.Find("Flame").GetComponent<SpriteRenderer>().enabled = isNeedToShow;
        return isNeedToShow;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Bullet")
        {
            Instantiate(particlesDestroy, transform.TransformPoint(Vector3.zero), Quaternion.identity, GameObject.FindWithTag("ParticlesParent").transform);
            if (collision.gameObject.tag == "AlienBullet")
                Destroy(collision.gameObject);
            gameController.DecreaseLife();
            Destroy(gameObject);
        }
    }
}
