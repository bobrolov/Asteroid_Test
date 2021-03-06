using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject ship;
    [SerializeField]
    private GameObject asteroidLarge;
    [SerializeField]
    private GameObject asteroidMedium;
    [SerializeField]
    private GameObject asteroidSmall;
    [SerializeField]
    private GameObject alienShip;
    [SerializeField]
    private GameObject scoreText;
    [SerializeField]
    private GameObject liveField;
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private GameObject startScreen;

    [Header("Сколько очков получает игрок")]
    [SerializeField]
    private int asteroidLargeScore = 20;
    [SerializeField]
    private int asteroidMediumScore = 50;
    [SerializeField]
    private int asteroidSmallScore = 100;
    [SerializeField]
    private int alienScore = 500;

    [SerializeField]
    private int asteroidIncrease = 2;
    [SerializeField]
    private int asteroidStartWave = 4;
    [SerializeField]
    private float alienSpawnTime = 10f;
    private float alienSpawnTimer = 20f;

    [SerializeField]
    public float xBorder = 44f;
    [SerializeField]
    public float yBorder = 24f;
    [SerializeField]
    private float borderSpawn = 2f;

    [SerializeField]
    private AudioSource asteroidDestroySound;
    [SerializeField]
    private AudioSource shipDestroySound;
    [SerializeField]
    private AudioSource gameOverSound;

    private int score;
    private int scoreAddLives;
    private int lives;
    private int wave;
    private int objectCounter;
    private bool isAlienCreate = true;
    // Start is called before the first frame update
    void Start()
    {
        SpawnAsteroids();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            GamePause();
        SpawnAlien();
    }

    public void AsteroidDestroy (string asteroidTag, Transform transform)
    {
        Debug.Log("Destroy");
        string newSize = "";
        if (asteroidTag == "AsteroidLarge")
        {
            score += asteroidLargeScore;
            newSize = "medium";
            objectCounter++;
        }
        else if (asteroidTag == "AsteroidMedium")
        {
            score += asteroidMediumScore;
            newSize = "small";
            objectCounter++;
        }
        else if (asteroidTag == "AsteroidSmall")
        {
            score += asteroidSmallScore;
            objectCounter--;
        }
        if ((newSize == "medium") || (newSize == "small"))
        {
            AsteroidCreate(newSize, transform.position, Quaternion.Euler(0, 0, Random.Range(30f, 70f)));
            AsteroidCreate(newSize, transform.position, Quaternion.Euler(0, 0, Random.Range(-70f, -30f)));
        }
        scoreText.GetComponent<Text>().text = score.ToString();
        CheckAddLive();
        asteroidDestroySound.Play();
        CheckObjectCounter();
    }

    public void AlienDestroy()
    {
        score += alienScore;
        scoreText.GetComponent<Text>().text = score.ToString();
        CheckAddLive();
        asteroidDestroySound.Play();
        isAlienCreate = false;
        objectCounter--;
        CheckObjectCounter();
        
    }
    void AsteroidCreate(string size, Vector3 position, Quaternion angle)
    {
        GameObject localAsteroid;
        if (size == "large")
            localAsteroid = Instantiate(asteroidLarge, position, angle, GameObject.FindWithTag("Asteroid_Parent").transform);
        else if (size == "medium")
            localAsteroid = Instantiate(asteroidMedium, position, angle, GameObject.FindWithTag("Asteroid_Parent").transform);
        else 
            localAsteroid = Instantiate(asteroidSmall, position, angle, GameObject.FindWithTag("Asteroid_Parent").transform);
        localAsteroid.transform.GetChild(Random.Range(0, localAsteroid.transform.childCount)).gameObject.SetActive(true);
    }

    void SpawnAlien ()
    {
        if (!isAlienCreate)
        {
            if (alienSpawnTimer > 0)
            {
                alienSpawnTimer -= Time.deltaTime;
            }
            else
            {
                Instantiate(alienShip, GetRandomSpawnPosition(), Quaternion.identity, GameObject.FindWithTag("Asteroid_Parent").transform);
                isAlienCreate = true;
                alienSpawnTimer = alienSpawnTime * Random.Range(1f, 1.5f);
                objectCounter++;
            }
        }
        
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        DestroyAsteroids();
        score = 0;
        scoreAddLives = 10000;
        lives = 3;
        wave = 0;
        isAlienCreate = false;
        
        scoreText.GetComponent<Text>().text = "00";
        scoreText.SetActive(true);
        liveField.transform.GetChild(0).gameObject.SetActive(true);
        liveField.transform.GetChild(1).gameObject.SetActive(true);
        liveField.transform.GetChild(2).gameObject.SetActive(true);
        liveField.SetActive(true);

        Instantiate(ship);
        SpawnAsteroids();
    }
    void DestroyAsteroids()
    {
        foreach (GameObject asteroid in GameObject.FindGameObjectsWithTag("AsteroidLarge"))
            GameObject.Destroy(asteroid);
        foreach (GameObject asteroid in GameObject.FindGameObjectsWithTag("AsteroidMedium"))
            GameObject.Destroy(asteroid);
        foreach (GameObject asteroid in GameObject.FindGameObjectsWithTag("AsteroidSmall"))
            GameObject.Destroy(asteroid);
    }
    public bool DecreaseLife()
    {
        shipDestroySound.Play();
        if (lives > 1)
        {
            liveField.transform.GetChild(3 - lives).gameObject.SetActive(false);
            lives--;
            return true;
        }
        else
        {
            GameOver();
            return false;
        }
    }
    void IncreaseLife()
    {
        if (lives <3)
        {
            lives++;
            liveField.transform.GetChild(3 - lives).gameObject.SetActive(true);
        }
    }

    void CheckAddLive ()
    {
        if (score >= scoreAddLives)
        {
            IncreaseLife();
            scoreAddLives += 10000;
        }
    }
    void GameOver()
    {
        liveField.SetActive(false);
        scoreText.SetActive(false);
        gameOverScreen.SetActive(true);
        gameOverScreen.transform.GetChild(1).gameObject.GetComponent<Text>().text = "SCORE: " + score.ToString();
        gameOverSound.Play();

    }

    void GamePause()
    {
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
    }

    public void GameUnPause()
    {
        Time.timeScale = 1f;
    }
    public void Quit()
    {
        Application.Quit();
    }

    void SpawnAsteroids()
    {
        objectCounter = asteroidStartWave + wave * asteroidIncrease;
        for (int i = 0; i < objectCounter; i++)
        {
            AsteroidCreate("large", GetRandomSpawnPosition(), Quaternion.Euler(0, 0, Random.Range(0f,359.9f)));
        }
    }

    void CheckObjectCounter ()
    {
        if (objectCounter == 0)
        {
            wave++;
            SpawnAsteroids();
        }
    }
    Vector3 GetRandomSpawnPosition()
    {
        int selectBorder = Random.Range(0, 3);
        float xPos;
        float yPos;
        if (selectBorder == 0)
        {
            xPos = Random.Range(-xBorder, xBorder);
            yPos = Random.Range(yBorder - borderSpawn, yBorder);
        }
        else if (selectBorder == 1)
        {
            xPos = Random.Range(-xBorder, xBorder);
            yPos = Random.Range(-yBorder, -yBorder + borderSpawn);
        }
        else if (selectBorder == 2)
        {
            xPos = Random.Range(xBorder-borderSpawn, xBorder);
            yPos = Random.Range(-yBorder + borderSpawn, yBorder - borderSpawn);
        }
        else
        {
            xPos = Random.Range(-xBorder, -xBorder + borderSpawn);
            yPos = Random.Range(-yBorder + borderSpawn, yBorder - borderSpawn);
        }
        return new Vector3(xPos, yPos, 0);
    }
}
