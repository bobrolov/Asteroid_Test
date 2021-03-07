using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour
{
    [Header("Параметры перемещения")]
    [SerializeField]
    private float speed = 300f;


    [Header("Параметры стрельбы")]
    [SerializeField]
    private GameObject alienBullet;
    [SerializeField]
    private float shootCooldown = 3f;
    private float shootTimer;

    [SerializeField]
    private float shootRadius = 1.5f;

    [SerializeField]
    private float changeDirectionTime = 2f;
    private float changeDirectionTimer = 0;

    [SerializeField]
    private GameObject particlesDestroy;

    [SerializeField]
    private float chanceToShootPlayer = 0.2f;

    [SerializeField]
    public float chanceAlienToEscape = 0.1f;

    private float xBorder;
    private float yBorder;




    private GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        shootTimer = shootCooldown * 2;
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        xBorder = gameController.xBorder;
        yBorder = gameController.yBorder;

        Invoke("AlienSoundStop", 0.8f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Shoot();
        Movement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collisionTag = collision.gameObject.tag;
        if (collisionTag != "AlienBullet")
        {
            if (collisionTag == "Bullet")
                Destroy(collision.gameObject);
            Instantiate(particlesDestroy, transform.TransformPoint(Vector3.zero), Quaternion.identity, GameObject.FindWithTag("ParticlesParent").transform);
            
            gameController.AlienDestroy(tag,(collisionTag == "Bullet") || (collisionTag == "Player"));
            Destroy(gameObject);
        }
    }

    void Movement()
    {

        if (changeDirectionTimer > 0)
        {
            changeDirectionTimer -= Time.deltaTime;
        }
        else
        {
            float angle = Random.Range(0f, 359.9f);
           // GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(speed*Mathf.Sin(angle)*Random.Range(1f,1.2f),speed*Mathf.Cos(angle)*Random.Range(1f, 1.2f)));
            changeDirectionTimer = changeDirectionTime * Random.Range(1f, 1.5f);
        }
    }

    void Shoot()
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            if (shootTimer > 0)
            {
                shootTimer -= Time.deltaTime;
            }
            else
            {
                Quaternion rotationToShoot;
                Vector3 positionToShoot;
                if (Random.Range(0f, 1f) < chanceToShootPlayer)
                {
                    rotationToShoot = GetRotationToPlayer();
                    positionToShoot = Vector3.MoveTowards(transform.position, GameObject.FindWithTag("Player").gameObject.transform.position, shootRadius);
                }
                else
                {
                    float angle = Random.Range(0f, 359.9f);
                    rotationToShoot = Quaternion.Euler(0, 0, angle);
                    positionToShoot = transform.TransformPoint(new Vector3(shootRadius * Mathf.Sin(angle), shootRadius * Mathf.Cos(angle), 0));
                }
                Instantiate(alienBullet, positionToShoot, rotationToShoot, GameObject.FindWithTag("BulletsAlien").transform);
                //GetComponent<AudioSource>().Play();
                shootTimer = shootCooldown * Random.Range(1f, 1.8f);
            }
        }
    }

    Quaternion GetRotationToPlayer()
    {
        Vector3 targetPos = GameObject.FindWithTag("Player").gameObject.transform.position;
        Vector2 vectorToTarget = (targetPos - transform.position);
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        return Quaternion.Euler(0, 0, angle);
        //return Quaternion.FromToRotation(transform.position, targetPos)*transform.rotation;
        //return Quaternion.AngleAxis(Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg, Vector3.back);
        //return Quaternion.LookRotation
    }

    void AlienSoundStop ()
    {
        GetComponent<AudioSource>().Stop();
    }

    public void AlienCanEscape ()
    {
        if (Random.Range(0,1f) < chanceAlienToEscape)
        {
            gameController.AlienEscape();
            Destroy(gameObject);
        }
    }
}
