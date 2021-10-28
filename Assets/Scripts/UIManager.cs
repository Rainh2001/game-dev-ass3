using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    public GameObject lifeHolder;
    public static int lives = 3;
    [SerializeField]
    public Text scoreText;
    public static int score = 0;
    [SerializeField]
    public Text timerText;
    public static float timer;
    [SerializeField]
    public Text ghostTimerText;
    public static int ghostTimer = 0;
    public Text countdownText;

    private int timerCounter = 0;
    private float countdownTimer;
    public static bool countingDown = true;

    public static bool gameOver = false;
    private bool gameOverInitialized = false;

    private void Awake() {
        gameOver = false;
        gameOverInitialized = false;
        countingDown = true;
        timerCounter = 0;
        ghostTimer = 0;
        score = 0;
        lives = 3;
        timer = 0.0f;
        ComponentManager.uIManager = this;
        ghostTimerText.text = "";
        Time.timeScale = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(countingDown){
            countdownTimer += Time.unscaledDeltaTime;
            if(countdownTimer - timerCounter >= 1){
                timerCounter++;
                if(timerCounter == 3){
                    countdownText.text = "GO!";
                } else if(timerCounter == 4){
                    countdownText.text = "";
                    countingDown = false;
                    Time.timeScale = 1.0f;
                    countdownTimer = 0;
                    timerCounter = 0;
                } else {
                    countdownText.text = 3 - timerCounter + "";
                }
            }
        }

        timer = Time.timeSinceLevelLoad;
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        timeText = timeText.Substring(0, 8);

        if(gameOver && !gameOverInitialized){
            ComponentManager.audioManager.audioSource.loop = false;
            gameOverInitialized = true;
            countdownText.text = "Game Over";
            Time.timeScale = 0.0f;

            float previousLowTime= 0.0f;
            if(PlayerPrefs.HasKey("time")){
                previousLowTime = PlayerPrefs.GetFloat("time");
            }
            int previousHighscore = 0;
            if(PlayerPrefs.HasKey("highscore")){
                previousHighscore = PlayerPrefs.GetInt("highscore");
            }

            if(score > previousHighscore || (score == previousHighscore && timer < previousLowTime && timer != 0.0f)){
                PlayerPrefs.SetInt("highscore", score);
                PlayerPrefs.SetFloat("time", timer);
            }

        }

        if(gameOver){
            countdownTimer += Time.unscaledDeltaTime;
            if(countdownTimer - timerCounter >= 1){
                timerCounter++;
                if(timerCounter == 3){
                    SceneLoader.loadStartScreen();
                }
            }
        }

        scoreText.text = "Score: " + score;
        timerText.text = timeText;
        if(ghostTimer != 0){
            ghostTimerText.text = "Scared: " + ghostTimer;
        } else {
            ghostTimerText.text = "";
        }

    }

    public void loseLife(){
        if(lives > 0) Destroy(lifeHolder.transform.Find("Life" + lives).gameObject);
        lives--;
        if(lives == 0){
            gameOver = true;
        }
    }
}
