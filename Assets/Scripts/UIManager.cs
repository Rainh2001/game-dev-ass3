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

    private void Awake() {
        ComponentManager.uIManager = this;
        ghostTimerText.text = "";
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
