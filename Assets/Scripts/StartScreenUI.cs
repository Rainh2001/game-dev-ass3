using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenUI : MonoBehaviour
{

    public Text highscoreText;
    public Text timeText;
    private int highscore = 0;
    private string time = "00:00:00";

    void Awake(){

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
            time = PlayerPrefs.GetString("time");
        }

        highscoreText.text = "Highscore: " + highscore;
        timeText.text = "Best Time: " + time;
    }
}
