using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [SerializeField]
    private AudioSource asteroidDestroySound;
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * Random.Range(20f, 150f));
        GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-20f, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CollisionDetect (Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Bullet"))
        {
            Destroy(collision.gameObject);

            asteroidDestroySound.Play();

            Destroy(gameObject);

        }
    }

}
