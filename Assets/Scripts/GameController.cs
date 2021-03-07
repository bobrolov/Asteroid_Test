using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Префабы")]
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
    private GameObject alienSmallShip;

    [Header("Игровые поля")]
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
    private int alienScore = 200;
    [SerializeField]
    private int alienSmallScore = 1000;
    [SerializeField]
    private int hardScore = 10000;

    [SerializeField]
    private int asteroidIncrease = 2;
    [SerializeField]
    private int asteroidStartWave = 4;


    [SerializeField]
    private float startAlienSpawnTime = 10f;
    [SerializeField]
    private float hardAlienSpawnTime = 4f;
    private float alienSpawnTimer;
    [SerializeField]
    private float startChanceToSpawnSmallAlien = 0.2f;
    [SerializeField]
    private float hardChanceToSpawnSmallAlien = 0.8f;


    [SerializeField]
    public float xBorder = 44f;
    [SerializeField]
    public float yBorder = 24f;
    [SerializeField]
    private float borderSpawn = 2f;

    [SerializeField]
    private GameObject asteroidDestroySound;
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

    public void StartGame()
    {
        Time.timeScale = 1f;
        DestroyAsteroids();
        score = 0;
        scoreAddLives = 10000;
        lives = 3;
        wave = 0;
        isAlienCreate = false;
        alienSpawnTimer = startAlienSpawnTime * Random.Range(1.2f,1.6f);


        scoreText.GetComponent<Text>().text = "00";
        scoreText.SetActive(true);
        liveField.transform.GetChild(0).gameObject.SetActive(true);
        liveField.transform.GetChild(1).gameObject.SetActive(true);
        liveField.transform.GetChild(2).gameObject.SetActive(true);
        liveField.SetActive(true);

        GameObject.FindWithTag("BGMusic").GetComponent<BGMusicScript>().CanPlay(true);
        GameObject.FindWithTag("BGMusic").GetComponent<BGMusicScript>().PauseReset();

        Invoke("SpawnShip", 0.5f);
        SpawnAsteroids();
    }

    public void AsteroidDestroy (string asteroidTag, Vector3 position, bool increaseScore)
    {
        string newSize = "";
        if (asteroidTag == "AsteroidLarge")
        {
            if (increaseScore)
                score += asteroidLargeScore;
            newSize = "medium";
            objectCounter++;
            PlayAsteroidDestroySound("large");
        }
        else if (asteroidTag == "AsteroidMedium")
        {
            if (increaseScore)
                score += asteroidMediumScore;
            newSize = "small";
            objectCounter++;
            PlayAsteroidDestroySound("medium");
        }
        else if (asteroidTag == "AsteroidSmall")
        {
            if (increaseScore)
                score += asteroidSmallScore;
            objectCounter--;
            PlayAsteroidDestroySound("small");
        }
        if ((newSize == "medium") || (newSize == "small"))
        {
            AsteroidCreate(newSize, position, Quaternion.Euler(0, 0, Random.Range(30f, 70f)));
            AsteroidCreate(newSize, position, Quaternion.Euler(0, 0, Random.Range(-70f, -30f)));
        }
        scoreText.GetComponent<Text>().text = score.ToString();
        CheckAddLive();
        CheckObjectCounter();
    }

    public void AlienDestroy(string tagAlien, bool increaseScore)
    {
        if (increaseScore)
        {
            if (tagAlien == "Alien")
                score += alienScore;
            else
                score += alienSmallScore;
            scoreText.GetComponent<Text>().text = score.ToString();
            CheckAddLive();
        }
        PlayAsteroidDestroySound("medium");
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
                if (score < hardScore)
                {
                    if (Random.Range(0f,1f) < startChanceToSpawnSmallAlien)
                        Instantiate(alienSmallShip, GetRandomSpawnPosition(), Quaternion.identity, GameObject.FindWithTag("Aliens").transform);
                    else
                        Instantiate(alienShip, GetRandomSpawnPosition(), Quaternion.identity, GameObject.FindWithTag("Aliens").transform);
                    float coeficient = score / hardScore;
                    alienSpawnTimer = (startAlienSpawnTime * (1 - coeficient) + hardAlienSpawnTime * coeficient) * Random.Range(0.3f, 1.5f);
                }
                else
                {
                    if (Random.Range(0f, 1f) < hardChanceToSpawnSmallAlien)
                        Instantiate(alienSmallShip, GetRandomSpawnPosition(), Quaternion.identity, GameObject.FindWithTag("Aliens").transform);
                    else
                        Instantiate(alienShip, GetRandomSpawnPosition(), Quaternion.identity, GameObject.FindWithTag("Aliens").transform);

                    alienSpawnTimer = hardAlienSpawnTime * Random.Range(0.8f, 1.5f);
                }

                isAlienCreate = true;
                objectCounter++;
            }
        }
        
    }



    void DestroyAsteroids()
    {
        foreach (GameObject asteroid in GameObject.FindGameObjectsWithTag("AsteroidLarge"))
            GameObject.Destroy(asteroid);
        foreach (GameObject asteroid in GameObject.FindGameObjectsWithTag("AsteroidMedium"))
            GameObject.Destroy(asteroid);
        foreach (GameObject asteroid in GameObject.FindGameObjectsWithTag("AsteroidSmall"))
            GameObject.Destroy(asteroid);
        foreach (GameObject alien in GameObject.FindGameObjectsWithTag("Alien"))
            GameObject.Destroy(alien);
    }

    public void DecreaseLife()
    {
        
        if (lives > 1)
        {
            shipDestroySound.Play();
            liveField.transform.GetChild(3 - lives).gameObject.SetActive(false);
            lives--;
            Invoke("SpawnShip", 1);
        }
        else
            GameOver();
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
        GameObject.FindWithTag("BGMusic").GetComponent<BGMusicScript>().CanPlay(false);
    }

    void GamePause()
    {
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
        GameObject.FindWithTag("BGMusic").GetComponent<BGMusicScript>().CanPlay(false);
    }

    public void GameUnPause()
    {
        Time.timeScale = 1f;
        GameObject.FindWithTag("BGMusic").GetComponent<BGMusicScript>().CanPlay(true);
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
            Invoke("SpawnAsteroids", 1);
            GameObject.FindWithTag("BGMusic").GetComponent<BGMusicScript>().PauseReset();
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

    void SpawnShip()
    {
        Instantiate(ship);
    }

    void PlayAsteroidDestroySound (string size)
    {
        if (size == "large")
            asteroidDestroySound.transform.Find("Large").GetComponent<AudioSource>().Play();
        else if (size == "medium")
            asteroidDestroySound.transform.Find("Medium").GetComponent<AudioSource>().Play();
        else if (size == "small")
            asteroidDestroySound.transform.Find("Small").GetComponent<AudioSource>().Play();

    }

    public void AlienEscape ()
    {

        isAlienCreate = false;
        objectCounter--;
        CheckObjectCounter();
    }
}
