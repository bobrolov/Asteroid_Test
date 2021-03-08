using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnScript : MonoBehaviour
{
    public bool isCanSpawn = true;
    public int collisionCounter = 0;
    private float collisionResetTime = 1f;
    private float collisionResetTimer;

    private void Start()
    {
        collisionResetTimer = collisionResetTime;
    }
    void Update()
    {
        if (collisionCounter != 0)
        {
            if (collisionResetTimer > 0)
                collisionResetTimer -= Time.deltaTime;
            else
            {
                isCanSpawn = true;
                collisionCounter = 0;
                collisionResetTimer = collisionResetTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag != "Bullet") && (collision.tag != "Player"))
        {
            isCanSpawn = false;
            collisionCounter++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.tag != "Bullet") && (collision.tag != "Player"))
        {
            collisionCounter--;
            if (collisionCounter == 0)
            {
                isCanSpawn = true;
                collisionResetTimer = collisionResetTime;
            }
        }
    }
}
