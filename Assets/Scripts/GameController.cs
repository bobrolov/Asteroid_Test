using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    #region VARIABLES

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

    [Header("Звуковые поля")]
    [SerializeField]
    private GameObject asteroidDestroySound;
    [SerializeField]
    private AudioSource shipDestroySound;
    [SerializeField]
    private AudioSource gameOverSound;

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
    private int score;
    private int scoreAddLives;

    [Header("Параметры пришельцев")]
    [SerializeField]
    private float startAlienSpawnTime = 10f;
    [SerializeField]
    private float hardAlienSpawnTime = 4f;
    private float alienSpawnTimer;
    private bool isAlienCreate = true;
    [SerializeField]
    private float startChanceToSpawnSmallAlien = 0.2f;
    [SerializeField]
    private float hardChanceToSpawnSmallAlien = 0.8f;

    [Header("Параметры края экрана")]
    public float xBorder;
    public float yBorder;
    [SerializeField]
    private float borderSpawn = 2f;

    [Header("Параметры игровые")]
    [SerializeField]
    private int asteroidIncrease = 1;
    [SerializeField]
    private int asteroidStartWave = 4;
    private int lives;
    private int wave;
    private int objectCounter;
    private bool isNeedToSpawnShip = false;

    #endregion

    #region START_UPDATE

    void Start()
    {
        SpawnAsteroids();
        GetBorders();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            GamePause();
        SpawnAlienRoutine();
        SpawnShipRoutine();
    }

    #endregion

    #region GAME_FUNCTIONS

    /*
     * Начать игру
     */
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

    /*
     * Закончить игру
     */
    void GameOver()
    {
        liveField.SetActive(false);
        scoreText.SetActive(false);
        gameOverScreen.SetActive(true);
        gameOverScreen.transform.GetChild(1).gameObject.GetComponent<Text>().text = "SCORE: " + score.ToString();
        gameOverSound.Play();
        GameObject.FindWithTag("BGMusic").GetComponent<BGMusicScript>().CanPlay(false);
    }

    /*
     * Игровая пауза
     */
    void GamePause()
    {
        if (!gameOverScreen.activeSelf)
        {
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            GameObject.FindWithTag("BGMusic").GetComponent<BGMusicScript>().CanPlay(false);
        }
    }

    /*
     * Возвращение из игровой паузы
     */
    public void GameUnPause()
    {
        Time.timeScale = 1f;
        GameObject.FindWithTag("BGMusic").GetComponent<BGMusicScript>().CanPlay(true);
    }

    /*
     * Выход из игры
     */
    public void Quit()
    {
        Application.Quit();
    }

    /*
     * Проверить количество игровых объектов
     */
    void CheckObjectCounter()
    {
        if (objectCounter == 0)
        {
            wave++;
            Invoke("SpawnAsteroids", 1);
            GameObject.FindWithTag("BGMusic").GetComponent<BGMusicScript>().PauseReset();
        }
    }

    #endregion

    #region PLAYER_FUNCTIONS

    /*
     * Заспавнить корабль игрока
     */
    void SpawnShip()
    {
        isNeedToSpawnShip = true;
    }

    /*
     * Обработчик рутины спавна корабля
     */
    void SpawnShipRoutine()
    {
        if (isNeedToSpawnShip)
            if (GameObject.FindWithTag("PlayerSpawnZone").GetComponent<PlayerSpawnScript>().isCanSpawn)
            {
                isNeedToSpawnShip = false;
                Instantiate(ship);
            }
    }

    /*
     * Обработать уменьшение жизней игрока
     */
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

    /*
     * Обработать увеличение жизней игрока
     */
    void IncreaseLife()
    {
        if (lives < 3)
        {
            lives++;
            liveField.transform.GetChild(3 - lives).gameObject.SetActive(true);
        }
    }

    /*
     * Проверка на увеличение жизней игрока
     */
    void CheckAddLive()
    {
        if (score >= scoreAddLives)
        {
            IncreaseLife();
            scoreAddLives += 10000;
        }
    }

    #endregion

    #region ASTEROIDS_FUNCTIONS

    /*
     * Создать астероид
     * 
     * @param {string size} Размер создаваемого астероида, large, medium, small
     * @param {Vector3 position} Позиция создаваемого астероида
     * @param {Quaternion angle} Вращение создаваемого астероида
     */
    void AsteroidCreate(string size, Vector3 position, Quaternion angle)
    {
        GameObject localAsteroid;
        if (size == "large")
            localAsteroid = Instantiate(asteroidLarge, position, angle, GameObject.FindWithTag("Asteroids").transform);
        else if (size == "medium")
            localAsteroid = Instantiate(asteroidMedium, position, angle, GameObject.FindWithTag("Asteroids").transform);
        else
            localAsteroid = Instantiate(asteroidSmall, position, angle, GameObject.FindWithTag("Asteroids").transform);
        localAsteroid.transform.GetChild(Random.Range(0, localAsteroid.transform.childCount)).gameObject.SetActive(true);
    }

    /*
     * Заспавнить астероиды, согласно текущей волне
     */
    void SpawnAsteroids()
    {
        objectCounter = asteroidStartWave + wave * asteroidIncrease;
        for (int i = 0; i < objectCounter; i++)
        {
            AsteroidCreate("large", GetRandomSpawnPosition(), Quaternion.Euler(0, 0, Random.Range(0f, 359.9f)));
        }
    }

    /*
     * Обработать разрушение астероида
     * 
     * @param {string asteroidTag} Размер разрушаемого астероида AsteroidLarge, AsteroidMedium, AsteroidSmall
     * @param {Vector3 position} Позиция где был разрушен астероид
     * @param {bool increaseScore} Логическая переменная, чтобы понять нужно ли прибавлять очки за уничтожение
     */
    public void AsteroidDestroy(string asteroidTag, Vector3 position, bool increaseScore)
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

    /*
     * Проиграть звук разрушения астероида
     * 
     * @param {string size} Размер астероида, large, medium, small
     */
    void PlayAsteroidDestroySound(string size)
    {
        if (size == "large")
            asteroidDestroySound.transform.Find("Large").GetComponent<AudioSource>().Play();
        else if (size == "medium")
            asteroidDestroySound.transform.Find("Medium").GetComponent<AudioSource>().Play();
        else if (size == "small")
            asteroidDestroySound.transform.Find("Small").GetComponent<AudioSource>().Play();
    }

    /*
     * Разрушить все астероиды
     */
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

    #endregion

    #region ALIEN_FUNCTIONS

    /*
     * Заспавнить корабль пришельцев
     */
    void SpawnAlienRoutine()
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
                    if (Random.Range(0f, 1f) < startChanceToSpawnSmallAlien)
                        Instantiate(alienSmallShip, GetRandomSpawnPosition(), Quaternion.identity);
                    else
                        Instantiate(alienShip, GetRandomSpawnPosition(), Quaternion.identity);
                    float coeficient = score / hardScore;
                    alienSpawnTimer = (startAlienSpawnTime * (1 - coeficient) + hardAlienSpawnTime * coeficient) * Random.Range(0.3f, 1.5f);
                }
                else
                {
                    if (Random.Range(0f, 1f) < hardChanceToSpawnSmallAlien)
                        Instantiate(alienSmallShip, GetRandomSpawnPosition(), Quaternion.identity);
                    else
                        Instantiate(alienShip, GetRandomSpawnPosition(), Quaternion.identity);
                    alienSpawnTimer = hardAlienSpawnTime * Random.Range(0.8f, 1.5f);
                }
                isAlienCreate = true;
                objectCounter++;
            }
        }
    }

    /*
     * Обработать разрушение корабля пришельцев
     * 
     * @param {string tagAlien} Тэг по которому можно понять большой или маленький пришелец
     * @param {bool increaseScore} Логическая переменная, чтобы понять нужно ли прибавлять очки за уничтожение
     */
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

    /*
     * Обработать побег корабля пришельцев за край экрана
     */
    public void AlienEscape()
    {
        isAlienCreate = false;
        objectCounter--;
        CheckObjectCounter();
    }

    #endregion

    #region COMMON_FUNCTIONS

    /*
     * Получить случайную позицию для спавна объекта
     * 
     * @return Возвращает случайную позицию в формате Vector3
     */
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
            xPos = Random.Range(xBorder - borderSpawn, xBorder);
            yPos = Random.Range(-yBorder + borderSpawn, yBorder - borderSpawn);
        }
        else
        {
            xPos = Random.Range(-xBorder, -xBorder + borderSpawn);
            yPos = Random.Range(-yBorder + borderSpawn, yBorder - borderSpawn);
        }
        return new Vector3(xPos, yPos, 0);
    }
    /*
     * Получить значение границ экрана
     */
    void GetBorders()
    {
        Vector3 borders = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        xBorder = borders.x;
        yBorder = borders.y;
    }

    #endregion
}
