using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{

    private Button button;

    void Awake(){
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadLevel1(){
        SceneManager.LoadSceneAsync(1);
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameManager.currentGameState = GameManager.GameState.Level1;
    }

    public void loadLevel2(){
        SceneManager.LoadSceneAsync(2);
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameManager.currentGameState = GameManager.GameState.Level2;
    }

    public static void loadStartScreen(){
        SceneManager.LoadSceneAsync(0);
        GameManager.currentGameState = GameManager.GameState.Start;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if(scene.buildIndex == 1 || scene.buildIndex == 2){
            button = GameObject.FindWithTag("QuitButton").GetComponent<Button>();
            button.onClick.AddListener(loadStartScreen);
        } else if(scene.buildIndex == 0){
            Time.timeScale = 1.0f;
        }
    }
}
