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
    }

    public void loadStartScreen(){
        SceneManager.LoadSceneAsync(0);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        if(scene.buildIndex == 1){
            button = GameObject.FindWithTag("QuitButton").GetComponent<Button>();
            button.onClick.AddListener(loadStartScreen);
        }
    }
}
