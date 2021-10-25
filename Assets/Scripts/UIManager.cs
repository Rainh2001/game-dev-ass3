using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private GameObject lifeHolder;
    public static int lives = 3;
    [SerializeField]
    private Text scoreText;
    public static int score = 0;
    [SerializeField]
    private Text timerText;
    public static string timer = "00:00:00";
    [SerializeField]
    private Text ghostTimerText;
    public static string ghostTimer = "00:00:00";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        timerText.text = timer;
        ghostTimerText.text = ghostTimer;
    }
}
