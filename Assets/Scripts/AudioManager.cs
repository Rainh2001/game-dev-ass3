using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip backgroundIntro;
    public AudioClip backgroundNormal;
    public AudioClip backgroundScared;

    public enum MusicState { Normal, Scared }

    public MusicState musicState = MusicState.Normal;

    void Awake(){
        ComponentManager.audioManager = this;
        audioSource = gameObject.GetComponent<AudioSource>();
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
        if(!audioSource.isPlaying && musicState == MusicState.Normal){
            audioSource.clip = backgroundNormal;
            audioSource.volume = 0.5f;
            audioSource.loop = true;
            audioSource.Play();
        }

    }

    public void changeMusicState(MusicState state){
        switch(state){
            case MusicState.Normal: audioSource.clip = backgroundNormal; break;
            case MusicState.Scared: audioSource.clip = backgroundScared; break;
        }
        audioSource.volume = 0.5f;
        audioSource.loop = true;
        audioSource.Play();

        musicState = state;
    }
}
