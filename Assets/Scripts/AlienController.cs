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
    private float shootTimer = 6f;

    [SerializeField]
    private float shootRadius = 1.5f;

    [SerializeField]
    private float changeDirectionTime = 2f;
    private float changeDirectionTimer = 0;

    public Vector3 temp;

    private GameController gameController;
    // Start is called before the first frame update
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            gameController.AlienDestroy();
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
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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
                Quaternion rotationToPlayer = GetRotationToPlayer();
                Vector3 positionShoot = Vector3.MoveTowards(transform.position, GameObject.FindWithTag("Player").gameObject.transform.position, shootRadius);
                Instantiate(alienBullet, positionShoot, rotationToPlayer, GameObject.FindWithTag("Bullet_Parent").transform);
                //GetComponent<AudioSource>().Play();
                shootTimer = shootCooldown * Random.Range(1f, 1.8f);
            }
        }
    }

    Quaternion GetRotationToPlayer()
    {
        Vector3 targetPos = GameObject.FindWithTag("Player").gameObject.transform.position;
        temp = targetPos;
        Vector2 vectorToTarget = (targetPos - transform.position);
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90 + Random.Range(-10f, 10f);
        return Quaternion.Euler(0, 0, angle);
        //return Quaternion.FromToRotation(transform.position, targetPos)*transform.rotation;
        //return Quaternion.AngleAxis(Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg, Vector3.back);
        //return Quaternion.LookRotation
    }
}
