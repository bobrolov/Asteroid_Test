using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1f);
    }
}
