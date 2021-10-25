using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{

    public static GameObject[] ghostObjects = new GameObject[4];
    public static Animator[] ghostAnimators = new Animator[4];
    public static GhostController[] ghosts = new GhostController[4];

    public enum GhostState { Alive, Scared, Recovering, Dead }

    public GhostState ghostState = GhostState.Alive;
    private GhostState oldState = GhostState.Alive;
    private int index;

    void Awake(){
        index = int.Parse(gameObject.tag[gameObject.tag.Length - 1] + "") - 1;
        ghostObjects[index] = gameObject;
        ghostAnimators[index] = gameObject.GetComponent<Animator>();
        ghosts[index] = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(oldState != ghostState){
            switch(oldState){
                case GhostState.Alive: 
                    if(ghostState == GhostState.Scared){
                        oldState = ghostState;
                        ghostAnimators[index].SetTrigger("scared");
                    } else ghostState = oldState; 
                    break;
                case GhostState.Scared:
                    if(ghostState == GhostState.Recovering){
                        oldState = ghostState;
                        ghostAnimators[index].SetTrigger("recovering");
                    } else ghostState = oldState; 
                    break;
                case GhostState.Recovering:
                    if(ghostState == GhostState.Scared){
                        oldState = ghostState;
                        ghostAnimators[index].SetTrigger("scared");
                    } else if(ghostState == GhostState.Dead){
                        oldState = ghostState;
                        ghostAnimators[index].SetTrigger("dead");
                    } else if(ghostState == GhostState.Alive){
                        oldState = ghostState;
                        ghostAnimators[index].SetTrigger("alive");
                    } else ghostState = oldState; 
                    break;
                case GhostState.Dead:
                    if(ghostState == GhostState.Alive){
                        oldState = ghostState;
                        ghostAnimators[index].SetTrigger("alive");
                    } else ghostState = oldState; 
                    break;
                default: ghostState = oldState; break;
            }
        }
    }
}
