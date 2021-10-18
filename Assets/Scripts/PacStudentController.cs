using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{

    private int posX = 1;
    private int posY = 1;

    private KeyCode currentInput;
    private KeyCode lastInput;
    private float speed = 5.0f;
    private bool tweening = false;
    private bool initialized = false;

    private Animator animator;

    void Awake(){
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W)){
            lastInput = KeyCode.W;
            initialized = true;
        } else if(Input.GetKeyDown(KeyCode.A)){
            lastInput = KeyCode.A;
            initialized = true;
        } else if(Input.GetKeyDown(KeyCode.S)){
            lastInput = KeyCode.S;
            initialized = true;
        } else if(Input.GetKeyDown(KeyCode.D)){
            lastInput = KeyCode.D;
            initialized = true;
        }

        if(!tweening && initialized){
            bool playing = false;
            int newX = posX;
            int newY = posY;
            int direction = 0;

            switch(lastInput){
                case KeyCode.W: newY -= 1; direction = 0; break;
                case KeyCode.A: newX -= 1; direction = 3; break;
                case KeyCode.S: newY += 1; direction = 2; break;
                case KeyCode.D: newX += 1; direction = 1; break;
            }

            if(MapManager.isValidPosition(newX, newY)){
                if(!playing){
                    animator.enabled = true;
                }
                currentInput = lastInput;
                posX = newX;
                posY = newY;
                animator.SetInteger("direction", direction);
                StartCoroutine(MoveToSpot(MapManager.getPosition(newX, newY)));
                playing = true;
            } else {
                newX = posX;
                newY = posY;

                switch(currentInput){
                    case KeyCode.W: newY -= 1; direction = 0; break;
                    case KeyCode.A: newX -= 1; direction = 3; break;
                    case KeyCode.S: newY += 1; direction = 2; break;
                    case KeyCode.D: newX += 1; direction = 1; break;
                }

                if(MapManager.isValidPosition(newX, newY)){
                    if(!playing){
                        animator.enabled = true;
                    }
                    posX = newX;
                    posY = newY;
                    animator.SetInteger("direction", direction);
                    StartCoroutine(MoveToSpot(MapManager.getPosition(newX, newY)));
                    playing = true;
                }
            }

            if(!playing){
                animator.enabled = false;
            }
        }
    }

    

    IEnumerator MoveToSpot(Vector3 position) {
        float startTime = Time.time;
        float duration = Vector3.Distance(transform.position, position)/speed;
        float t = 0.0f;
        Vector3 startPos = transform.position;

        tweening = true;
        while (t < 1.0f){
            t = (Time.time - startTime)/duration;
            transform.position = Vector3.Lerp(startPos, position, t);
            yield return null;
        }

        transform.position = position;
        tweening = false;
        yield return null;
    }
}
