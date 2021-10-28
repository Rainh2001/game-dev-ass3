using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public static string timer = "00:00:00";
    [SerializeField]
    public Text ghostTimerText;
    public static int ghostTimer = 0;
    public Text countdownText;

    private int timerCounter = 0;
    private float countdownTimer;
    public static bool countingDown = true;
    private bool numCounting = true;

    private void Awake() {
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
                } else {
                    countdownText.text = 3 - timerCounter + "";
                }
            }
        }
        

        scoreText.text = "Score: " + score;
        timerText.text = timer;
        if(ghostTimer != 0){
            ghostTimerText.text = "Scared: " + ghostTimer;
        } else {
            ghostTimerText.text = "";
        }

    }

    public void loseLife(){
        if(lives > 0) Destroy(lifeHolder.transform.Find("Life" + lives).gameObject);
        lives--;
    }
}
