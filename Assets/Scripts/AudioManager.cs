using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip backgroundIntro;
    public AudioClip backgroundNormal;

    void Awake(){

        audioSource.clip = backgroundIntro;
        audioSource.Play();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(!audioSource.isPlaying){
            audioSource.clip = backgroundNormal;
            audioSource.loop = true;
            audioSource.Play();
        }

    }
}
