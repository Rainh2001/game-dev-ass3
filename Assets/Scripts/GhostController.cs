using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{

    public static GhostController[] ghosts = new GhostController[4];

    public enum GhostState { Alive, Scared, Recovering, Dead }


    // public GhostState ghostState = GhostState.Alive;
    private GhostState ghostState = GhostState.Alive;
    private int index;
    private Animator animator;

    void Awake(){
        
        index = int.Parse(gameObject.tag[gameObject.tag.Length - 1] + "") - 1;
        ghosts[index] = this;
        if(index == 0) ComponentManager.ghostController = this;
        animator = gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateGhostState(GhostState state){

        switch(state){
            case GhostState.Alive: ComponentManager.audioManager.changeMusicState(AudioManager.MusicState.Normal); break;
            case GhostState.Scared: ComponentManager.audioManager.changeMusicState(AudioManager.MusicState.Scared); break;
        }

        for(int i = 0; i < ghosts.Length; i++){
            if(ghosts[i].ghostState != state){
                switch(ghosts[i].ghostState){
                    case GhostState.Alive: 
                        if(state == GhostState.Scared){
                            updateToScared(i);
                        } 
                        break;
                    case GhostState.Scared:
                        if(state == GhostState.Recovering){
                            updateToRecovering(i);
                        }
                        break;
                    case GhostState.Recovering:
                        if(state == GhostState.Scared){
                            updateToScared(i);
                        } else if(state == GhostState.Dead){
                            updateToDead(i);
                        } else if(state == GhostState.Alive){
                            updateToAlive(i);
                        }  
                        break;
                    case GhostState.Dead:
                        if(state == GhostState.Alive){
                            updateToAlive(i);
                        }
                        break;
                }
            }
        }
    }

    private static void updateToScared(int i){
        ghosts[i].ghostState = GhostState.Scared;
        ghosts[i].animator.SetTrigger("scared");
    }
    private static void updateToAlive(int i){
        ghosts[i].ghostState = GhostState.Alive;
        ghosts[i].animator.SetTrigger("alive");
    }
    private static void updateToDead(int i){
        ghosts[i].ghostState = GhostState.Dead;
        ghosts[i].animator.SetTrigger("dead");
    }
    private static void updateToRecovering(int i){
        ghosts[i].ghostState = GhostState.Recovering;
        ghosts[i].animator.SetTrigger("recovering");
    }
}
