using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StartScreenUI : MonoBehaviour
{

    public Text highscoreText;
    public Text timeText;
    private int highscore = 0;
    private float time = 0.0f;

    void Awake(){
        highscore = 0;
        time = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.HasKey("highscore")){
            highscore = PlayerPrefs.GetInt("highscore");
        }
        if(PlayerPrefs.HasKey("time")){
            time = PlayerPrefs.GetFloat("time");
        }

        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        string lowestTime = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        lowestTime = lowestTime.Substring(0, 8);

        highscoreText.text = "Highscore: " + highscore;
        timeText.text = "Best Time: " + lowestTime;
    }
}
