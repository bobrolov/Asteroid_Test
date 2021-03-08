using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour
{
    #region VARIABLES

    [Header("Префабы")]
    [SerializeField]
    private GameObject particlesDestroy;

    [Header("Параметры перемещения")]
    [SerializeField]
    private float speed = 300f;
    [SerializeField]
    private float changeDirectionTime = 2f;
    private float changeDirectionTimer = 0;


    [Header("Параметры стрельбы")]
    [SerializeField]
    private GameObject alienBullet;
    [SerializeField]
    private float shootCooldown = 3f;
    private float shootTimer;
    [SerializeField]
    private float shootRadius = 1.5f;

    [Header("Вероятности")]
    [SerializeField]
    private float chanceToShootPlayer = 0.2f;
    [SerializeField]
    public float chanceAlienToEscape = 0.1f;

    //Игровой контроллер
    private GameController gameController;

    #endregion

    #region START_UPDATE

    void Start()
    {
        shootTimer = shootCooldown * 2;
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        Invoke("AlienSoundStop", 0.8f);
    }

    void Update()
    {
        Shoot();
        Movement();
    }

    #endregion

    #region ALIEN_FUNCTIONS

    /*
     * Обработать перемещение алиена
     */
    void Movement()
    {
        if (changeDirectionTimer > 0)
            changeDirectionTimer -= Time.deltaTime;
        else
        {
            float angle = Random.Range(0f, 359.9f);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * Mathf.Sin(angle) * Random.Range(1f, 1.2f), speed * Mathf.Cos(angle) * Random.Range(1f, 1.2f)));
            changeDirectionTimer = changeDirectionTime * Random.Range(1f, 1.5f);
        }
    }

    /*
     * Обработать стрельбу алиена
     */
    void Shoot()
    {
        if (GameObject.FindWithTag("Player") != null)
        {
            if (shootTimer > 0)
                shootTimer -= Time.deltaTime;
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
                shootTimer = shootCooldown * Random.Range(1f, 1.8f);
            }
        }
    }

    /*
     * Получить поворот на игрока
     * 
     * @return {Quaternion} Возвращает кватернион поворота на игрока
     */
    Quaternion GetRotationToPlayer()
    {
        Vector3 targetPos = GameObject.FindWithTag("Player").gameObject.transform.position;
        Vector2 vectorToTarget = (targetPos - transform.position);
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90 * Random.Range(0.9f, 1.1f);
        return Quaternion.Euler(0, 0, angle);
    }

    /*
     * Обработать столкновение
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collisionTag = collision.gameObject.tag;
        if (collisionTag != "AlienBullet" && collisionTag != "PlayerSpawnZone")
        {
            if (collisionTag == "Bullet")
                Destroy(collision.gameObject);
            Instantiate(particlesDestroy, transform.TransformPoint(Vector3.zero), Quaternion.identity, GameObject.FindWithTag("Particles").transform);
            gameController.AlienDestroy(tag,(collisionTag == "Bullet") || (collisionTag == "Player"));
            Destroy(gameObject);
        }
    }

    /*
     * Остановить звук
     */
    void AlienSoundStop ()
    {
        GetComponent<AudioSource>().Stop();
    }

    /*
     * Обработать побег 
     */
    public void AlienCanEscape ()
    {
        if (Random.Range(0,1f) < chanceAlienToEscape)
        {
            gameController.AlienEscape();
            Destroy(gameObject);
        }
    }

    #endregion
}
