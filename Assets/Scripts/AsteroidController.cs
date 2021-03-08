using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [Header("Префабы")]
    [SerializeField]
    private GameObject particlesDestroy;

    [Header("Параметры перемещения")]
    [SerializeField]
    private float minSpeed = 30f;
    [SerializeField]
    private float maxSpeed = 150f;

    //Игровой контроллер
    private GameController gameController;

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        GetComponent<Rigidbody2D>().AddForce(transform.up * Random.Range(minSpeed, maxSpeed));
    }

    public void CollisionDetect (Collider2D collision)
    {
        string collisionTag = collision.gameObject.tag;
        if ((collisionTag == "Bullet") || (collisionTag == "Player") || (collisionTag == "AlienBullet") || (collisionTag == "Alien") || (collisionTag == "AlienSmall"))
        {
            if ((collisionTag == "Bullet") || (collisionTag == "AlienBullet"))
                Destroy(collision.gameObject);
            Instantiate(particlesDestroy, transform.TransformPoint(Vector3.zero), Quaternion.identity, GameObject.FindWithTag("Particles").transform);
            gameController.AsteroidDestroy(tag, this.transform.position, ((collisionTag == "Bullet") || (collisionTag == "Player")));
            Destroy(gameObject);
        }
    }
}
