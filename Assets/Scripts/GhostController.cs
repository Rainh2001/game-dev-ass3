using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{

    public static GameObject[] ghostObjects = new GameObject[4];
    public static Animator[] ghostAnimators = new Animator[4];
    public static GhostController[] ghosts = new GhostController[4];

    public enum GhostState { Alive, Scared, Recovering, Dead }


    // public GhostState ghostState = GhostState.Alive;
    private GhostState ghostState = GhostState.Alive;
    private int index;
    private Animator animator;

    void Awake(){
        index = int.Parse(gameObject.tag[gameObject.tag.Length - 1] + "") - 1;
        // ghostObjects[index] = gameObject;
        // ghostAnimators[index] = gameObject.GetComponent<Animator>();
        ghosts[index] = this;
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

    public static void updateGhostState(GhostState state){
        for(int i = 0; i < ghosts.Length; i++){
            if(ghosts[i].ghostState != state){
                switch(ghosts[i].ghostState){
                    case GhostState.Alive: 
                        if(state == GhostState.Scared){
                            ghosts[i].ghostState = state;
                            ghosts[i].animator.SetTrigger("scared");
                        } 
                        break;
                    case GhostState.Scared:
                        if(state == GhostState.Recovering){
                            ghosts[i].ghostState = state;
                            ghosts[i].animator.SetTrigger("recovering");
                        }
                        break;
                    case GhostState.Recovering:
                        if(state == GhostState.Scared){
                            ghosts[i].ghostState = state;
                            ghosts[i].animator.SetTrigger("scared");
                        } else if(state == GhostState.Dead){
                            ghosts[i].ghostState = state;
                            ghosts[i].animator.SetTrigger("dead");
                        } else if(state == GhostState.Alive){
                            ghosts[i].ghostState = state;
                            ghosts[i].animator.SetTrigger("alive");
                        }  
                        break;
                    case GhostState.Dead:
                        if(state == GhostState.Alive){
                            ghosts[i].ghostState = state;
                            ghosts[i].animator.SetTrigger("alive");
                        }
                        break;
                }
            }
        }
    }
}
