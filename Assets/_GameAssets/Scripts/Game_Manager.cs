using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Game_Manager : MonoBehaviour
{
    [SerializeField] private Transform[] spawns;
    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject gameOverCamera;
    [Space(10)]
    [SerializeField] private GameObject ui_PlayerHUDObject;
    [SerializeField] private GameObject ui_GameOverObject;
    [SerializeField] private GameObject ui_DamageIndicator;
    //[SerializeField] private GameObject ui_PauseMenu;

    [Space(10)]
    [SerializeField] private TextMeshProUGUI ui_AmmoText;
    [SerializeField] private TextMeshProUGUI ui_AmmoReservText;
    [SerializeField] private TextMeshProUGUI ui_RoundText;
    [SerializeField] private TextMeshProUGUI ui_ScoreText;
    [SerializeField] private TextMeshProUGUI ui_HighscoreText;
    [SerializeField] private TextMeshProUGUI ui_SurvivedRounds;
    [SerializeField] private TextMeshProUGUI ui_ScoreAtEnd;

    [Space(10)]
    [SerializeField] private AudioSource EndRoundZombieSound;

    private int roundNumber;
    private int zombiesInMap;
    private int playerScore;
    private int avaiableScore;

    private GameObject player;

    private bool spawningDone;

    private Game_GunShoot gunShoot;
    private List<Class_Score> scores;

    public enum RoundState
    {
        Starting,
        OnGoing,
        Ending,
        Ended,
        PlayerDied
    }
    [Space(10)]
    public RoundState currentState;
    public Queue<Class_Score> scoreQueue;

    private void Start()
    {
        player = GameObject.Find("/Player");

        roundNumber = 1;
        playerScore = 0;
        avaiableScore = 0;
        zombiesInMap = 0;
        currentState = RoundState.Starting;

        scoreQueue = new Queue<Class_Score>();
        scores = new List<Class_Score>();
    }

    private void Update()
    {
        StateSwitcher();
        AddScore();
        UpdateUI();
        //Pause();
    }

    private void StateSwitcher()
    {
        switch (currentState)
        {
            case RoundState.Starting:

                RoundStart();
                currentState = RoundState.OnGoing;
                break;

            case RoundState.OnGoing:

                if (player.GetComponent<Game_PlayerHealth>().isPlayerDead()) currentState = RoundState.PlayerDied;
                if (HasRoundEnded()) currentState = RoundState.Ending;
                break;

            case RoundState.Ending:

                StartCoroutine(RoundEnding());
                currentState = RoundState.Ended;
                break;

            case RoundState.Ended:

                currentState = RoundState.Starting;
                break;

            case RoundState.PlayerDied:

                FPlayerHasDied();
                break;

            default:
                break;
        }
    }

    private void FPlayerHasDied()
    {
        if (!(PlayerPrefs.HasKey("Highscore")) || (PlayerPrefs.GetInt("Highscore") < playerScore)) PlayerPrefs.SetInt("Highscore", playerScore);
        mainCamera.SetActive(false);
        player.GetComponent<Game_PlayerMovement>().enabled = false;
        player.GetComponent<Game_PlayerCamera>().enabled = false;
        player.GetComponent<Game_PlayerHealth>().enabled = false;
        gameOverCamera.SetActive(true);
        ui_PlayerHUDObject.SetActive(false);
        ui_DamageIndicator.SetActive(false);
        ui_GameOverObject.SetActive(true);
        ui_HighscoreText.text = PlayerPrefs.GetInt("Highscore").ToString();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private IEnumerator SpawnZombies(float zombiesToSpawn, float zombieHealth)
    {
        for (int i = (int)zombiesToSpawn; i > 0; i--)
        {
            zombiesInMap += 1;
            yield return new WaitForSeconds(3f);
            int spawnIndex = Random.Range(0, spawns.Length);
            GameObject zombie = Instantiate(zombiePrefab, spawns[spawnIndex].position, new Quaternion());
            zombie.GetComponent<Game_ZombieHealth>().SetHealth(zombieHealth);
        }
        spawningDone = true;
    }

    private float ZombiesToSpawn()
    {
        if (roundNumber < 20)
        {
            int[] initialNumber = { 6, 8, 13, 18, 24, 27, 28, 28, 29, 33, 34, 36, 39, 41, 44, 47, 50, 53, 56 };
            return initialNumber[roundNumber - 1];
        }
        return Mathf.Round(.9f * roundNumber * roundNumber - .0029f * roundNumber + 23.958f);
    }

    private float ZombiesHealth()
    {
        if(roundNumber < 10)
        {
            int[] initialNumber = { 150, 250, 350, 450, 550, 650, 750, 850, 950 };
            return initialNumber[roundNumber - 1];
        }
        return 950 * Mathf.Pow(1.1f, roundNumber - 9);
    }

    public void DescreaseZombiesOnMap()
    {
        zombiesInMap -= 1;
    }

    private void RoundStart()
    {
        spawningDone = false;
        StartCoroutine(SpawnZombies(ZombiesToSpawn(), ZombiesHealth()));
    }

    private bool HasRoundEnded()
    {
        if ((zombiesInMap <= 0) && (spawningDone)) return true;
        return false;
    }

    private IEnumerator RoundEnding()
    {
        EndRoundZombieSound.Play();
        roundNumber += 1;
        yield return null;
    }

    public int GetRoundNumber()
    {
        return roundNumber;
    }

    private void AddScore()
    {
        if (scoreQueue.Count == 0) return;
        Class_Score scoreObj = scoreQueue.Dequeue();
        playerScore += scoreObj.scoreValue;
        avaiableScore += scoreObj.scoreValue;
        scores.Add(scoreObj);
    }

    public void AddToScoreQueue(Class_Score.ScoreID idParam, int scoreValueParam, string scoreDescParam)
    {
        scoreQueue.Enqueue(new Class_Score
        {
            id = idParam,
            scoreValue = scoreValueParam,
            scoreDesc = scoreDescParam
        });
    }

    public void RemoveScore(int scoreToRemove)
    {
        avaiableScore -= scoreToRemove;
    }

    public bool HasEnoughPoints(int toCompara)
    {
        if (toCompara <= avaiableScore) return true;
        return false;
    }

    private void UpdateUI()
    {
        ui_AmmoText.text = player.GetComponent<Game_PlayerWeapon>().GetCurrentEquipedGun().GetComponent<Game_GunShoot>().GetCurrentAmmo().ToString();
        ui_AmmoReservText.text = "/ "+player.GetComponent<Game_PlayerWeapon>().GetCurrentEquipedGun().GetComponent<Game_GunShoot>().GetCurrentReservAmmo().ToString();
        ui_RoundText.text = roundNumber.ToString();
        ui_SurvivedRounds.text = "you survived  "+  roundNumber.ToString() + " rounds";
        ui_ScoreAtEnd.text = "and got " + playerScore.ToString() + " points";
        ui_ScoreText.text = avaiableScore.ToString();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /*private void Pause()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (!ui_PauseMenu.activeInHierarchy)
            {
                Time.timeScale = 0;
                ui_PauseMenu.SetActive(true);
            }
            else if (ui_PauseMenu.activeInHierarchy)
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        ui_PauseMenu.SetActive(false);
    }*/
}
