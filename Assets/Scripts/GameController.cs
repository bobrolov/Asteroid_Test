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
    private GameObject scoreText;
    [SerializeField]
    private GameObject liveField;
    [SerializeField]
    private GameObject GameOverScreen;

    [Header("Сколько очков получает игрок")]
    [SerializeField]
    private int asteroidLargeScore = 20;
    [SerializeField]
    private int asteroidMediumScore = 50;
    [SerializeField]
    private int asteroidSmallScore = 100;

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
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Quit();
    }

    public void AsteroidDestroy (string asteroidTag, Transform transform)
    {
        Debug.Log("Destroy");
        string newSize = "";
        if (asteroidTag == "AsteroidLarge")
        {
            score += asteroidLargeScore;
            newSize = "medium";
        }
        else if (asteroidTag == "AsteroidMedium")
        {
            score += asteroidMediumScore;
            newSize = "small";
        }
        else if (asteroidTag == "AsteroidSmall")
        {
            score += asteroidSmallScore;
        }
        if ((newSize == "medium") || (newSize == "small"))
        {
            AsteroidCreate(newSize, transform.position, Quaternion.Euler(0, 0, Random.Range(75, 105)));
            AsteroidCreate(newSize, transform.position, Quaternion.Euler(0, 0, Random.Range(245, 295)));
        }
        scoreText.GetComponent<Text>().text = score.ToString();
        CheckAddLive();
        asteroidDestroySound.Play();
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
        localAsteroid.transform.GetChild(Random.Range(0, 2)).gameObject.SetActive(true);
    }

    public void StartGame()
    {
        DestroyAsteroids();
        score = 0;
        scoreAddLives = 10000;
        lives = 3;
        Instantiate(ship);
        scoreText.GetComponent<Text>().text = "00";
        scoreText.SetActive(true);
        liveField.transform.GetChild(0).gameObject.SetActive(true);
        liveField.transform.GetChild(1).gameObject.SetActive(true);
        liveField.transform.GetChild(2).gameObject.SetActive(true);
        liveField.SetActive(true);
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
        if (lives > 0)
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
        GameOverScreen.SetActive(true);
        GameOverScreen.transform.GetChild(1).gameObject.GetComponent<Text>().text = "SCORE: " + score.ToString();
        gameOverSound.Play();

    }
    public void Quit()
    {
        Application.Quit();
    }
}
