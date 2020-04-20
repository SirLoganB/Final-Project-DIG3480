using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;

    private bool gameOver;
    private bool gameWin;
    private bool restart;
    private int score;

    public AudioSource musicSource;
    public AudioClip clipBG;
    public AudioClip clipWin;
    public AudioClip clipLose;

    public GameObject otherGameObject;
    private BGScroller yetAnotherScript;
    public GameObject Bg;
    public GameObject BgWin;

    public GameObject boundary;

    void Start()
    {
        yetAnotherScript = otherGameObject.GetComponent<BGScroller>();

        gameOver = false;
        gameWin = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        if (gameOver)
        {
            restartText.text = "Press x for Restart";
            restart = true;
        }

        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            {
                for (int i = 0; i < hazardCount; i++)
                {
                    GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(hazard, spawnPosition, spawnRotation);
                    yield return new WaitForSeconds(spawnWait);
                }
            }
            yield return new WaitForSeconds(waveWait);
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
        if (score >= 150)
        {
            boundary.transform.position = new Vector3(0, 0, 26);
            StopCoroutine(SpawnWaves());
            musicSource.clip = clipWin;
            musicSource.Play();
            gameOverText.text = "YOU WIN! GAME CREATED BY LOGAN BERNATT";
            gameWin = true;
            Bg.SetActive(false);
            BgWin.SetActive(true);
            gameOver = true;
        }
    }

    void UpdateScore()
    {
        scoreText.text = "Points: " + score + "/ 150";
    }

    public void GameOver()
    {
        musicSource.clip = clipLose;
        musicSource.Play();
        gameOverText.text = "Game Over!";
        gameOver = true;
    }
}