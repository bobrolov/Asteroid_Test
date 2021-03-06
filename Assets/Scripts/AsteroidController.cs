using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [Header("Параметры перемещения")]
    [SerializeField]
    private float minSpeed = 30f;
    [SerializeField]
    private float maxSpeed = 150f;

    private GameController gameController;


    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        GetComponent<Rigidbody2D>().AddForce(transform.up * Random.Range(minSpeed, maxSpeed));
    }

    public void CollisionDetect (Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            Destroy(collision.gameObject);
            gameController.AsteroidDestroy(tag, this.transform);
            Destroy(gameObject);

        }
    }

}
