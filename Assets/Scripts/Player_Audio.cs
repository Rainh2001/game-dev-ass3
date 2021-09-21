using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Audio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip movingNoEating;

    void Awake(){
        audioSource.clip = movingNoEating;
        audioSource.loop = true;
    }

    void Start(){
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
