using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    private GameController gameController;

    [SerializeField]
    private float blinkTime = 0.6f;
    private float blinkTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (blinkTimer >= blinkTime)
        {
            if (gameObject.transform.GetChild(0).gameObject.activeSelf)
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
            else
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
            blinkTimer = 0;
        }
        else
            blinkTimer += Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            gameController.StartGame();
            gameObject.SetActive(false);
        }
    }
}
