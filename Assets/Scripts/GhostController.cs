using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{

    public static GhostController[] ghosts = new GhostController[4];

    public enum GhostState { Alive, Scared, Recovering, Dead }

    public static int timerCounter;
    private static float timer;
    private static bool timerActive = false;

    private static GhostState staticGhostState = GhostState.Alive;

    // public GhostState ghostState = GhostState.Alive;
    public GhostState ghostState = GhostState.Alive;
    private int index;
    private Animator animator;

    private static int ghostDeadCount = 0;
    private static bool ghostDead = false;
    private float speed;
    private float baseSpeed = 5.0f;
    private bool tweening;
    private bool inSpawn = true;
    private int posX;
    private int posY;
    private int previousX;
    private int previousY;
    private static int spawnX = 13;
    private static int spawnY = 14;
    private List<int> moveList = new List<int>();

    private int ghost4Direction = 0;
    private bool inPosition = true;


    void Awake(){
        index = int.Parse(gameObject.tag[gameObject.tag.Length - 1] + "") - 1;
        ghosts[index] = this;
        if(index == 0) ComponentManager.ghostController = this;
        animator = gameObject.GetComponent<Animator>();
        speed = 5.0f;   
        tweening = false;
        inSpawn = true;
        posY = spawnY;
        inPosition = true;
        switch(index){
            case 0: {
                posX = spawnX - 1; 
                moveList.Add(0);
                moveList.Add(1);
                moveList.Add(0);
                moveList.Add(0);
                break;
            }
            case 1: {
                posX = spawnX; 
                moveList.Add(0);
                moveList.Add(0);
                moveList.Add(0);
                break;
            }
            case 2: {
                posX = spawnX + 1; 
                moveList.Add(0);
                moveList.Add(0);
                moveList.Add(0);
                break;
            }
            case 3: {
                posX = spawnX + 2; 
                moveList.Add(2);
                moveList.Add(3);
                moveList.Add(2);
                moveList.Add(2);
                moveList.Add(1);
                moveList.Add(1);
                moveList.Add(1);
                moveList.Add(1);
                moveList.Add(0);
                moveList.Add(0);
                moveList.Add(0);
                moveList.Add(1);
                moveList.Add(1);
                moveList.Add(1);
                break;
            }
        }
        previousX = posX;
        previousY = posY;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Ghost Management, not a good way to do it, however it works and im happy with that lol
        if(index == 0){
            if(staticGhostState == GhostState.Scared && timerCounter < 10){
                ComponentManager.uIManager.ghostTimerText.gameObject.SetActive(true);
                timer += Time.deltaTime;
                if(timer - timerCounter >= 1){
                    timerCounter++;
                    int timeRemaining = 10 - timerCounter;
                    UIManager.ghostTimer = timeRemaining;

                    if(timeRemaining <= 3) {
                        updateGhostState(GhostState.Recovering);
                    }
                }
            } else if(timerActive){
                ComponentManager.uIManager.ghostTimerText.gameObject.SetActive(false);
                staticGhostState = GhostState.Alive;
                updateGhostState(GhostState.Alive);
                timerActive = false;
            }
        }

        // Ghost Movement AI
        if(!tweening){

            if(moveList.Count > 0 && (staticGhostState == GhostState.Scared || staticGhostState == GhostState.Recovering)){
                moveList.Clear();
            }

            if(moveList.Count > 0){
                int direction = moveList[0];
                moveList.RemoveAt(0);
                previousX = posX;
                previousY = posY;
                switch(direction){
                    case 0: posY--; animator.SetInteger("direction", 1); break;
                    case 1: posX++; animator.SetInteger("direction", 2); break;
                    case 2: posY++; animator.SetInteger("direction", 3); break;
                    case 3: posX--; animator.SetInteger("direction", 0); break;
                }
                StartCoroutine(MoveToSpot(MapManager.getPosition(posX, posY)));
            } else if(index == 0 || staticGhostState == GhostState.Scared || staticGhostState == GhostState.Recovering){

                // Ghost 1 AI
                List<Vector3> validPos = new List<Vector3>();
                List<int> validDir = new List<int>();
                float distanceFromPac = Vector3.Distance(transform.position, ComponentManager.pacStudentController.transform.position);

                for(int i = 0; i < 4; i++){
                    int newX = posX;
                    int newY = posY;
                    switch(i){
                        case 0: newY--; break;
                        case 1: newX++; break;
                        case 2: newY++; break;
                        case 3: newX--; break;
                    }
                    if(MapManager.isValidPosition(newX, newY) && !MapManager.isSpawnPosition(newX, newY)){
                        Vector3 newPos = MapManager.getPosition(newX, newY);
                        float newDistanceFromPac = Vector3.Distance(newPos, ComponentManager.pacStudentController.transform.position);
                        if(newDistanceFromPac >= distanceFromPac){
                            validPos.Add(newPos);
                            validDir.Add(i);
                        }
                    }
                }

                if(validDir.Count > 0){
                    int rand = Random.Range(0, validPos.Count);
                    previousX = posX;
                    previousY = posY;
                    switch(validDir[rand]){
                        case 0: posY--; animator.SetInteger("direction", 1); break;
                        case 1: posX++; animator.SetInteger("direction", 2); break;
                        case 2: posY++; animator.SetInteger("direction", 3); break;
                        case 3: posX--; animator.SetInteger("direction", 0); break;
                    }
                    StartCoroutine(MoveToSpot(validPos[rand]));
                } else {
                    stepBack();
                }
                

            } else if(index == 1){
                // Ghost 2 AI
                List<List<int>> junctionList = MapManager.getNearestJunctions(posX, posY);
                float distanceFromPac = Vector3.Distance(transform.position, ComponentManager.pacStudentController.transform.position);

                for(int i = junctionList.Count - 1; i >= 0; i--){
                    float junctionDistance = Vector3.Distance(ComponentManager.pacStudentController.transform.position, MapManager.getPosition(junctionList[i][0], junctionList[i][1]));
                    if(junctionDistance > distanceFromPac) junctionList.RemoveAt(i);
                }

                if(junctionList.Count > 0){
                    int rand = Random.Range(0, junctionList.Count);
                    previousX = posX;
                    previousY = posY;

                    int newX = junctionList[rand][0];
                    int newY = junctionList[rand][1];

                    int xDiff = newX - posX;
                    int yDiff = newY - posY;

                    int length = 0;
                    int direction = 0;
                    if(xDiff != 0){
                        length = Mathf.Abs(xDiff);
                        if(xDiff > 0){
                            direction = 1;
                        } else {
                            direction = 3;
                        }
                    }
                    if(yDiff != 0){
                        length = Mathf.Abs(yDiff);
                        if(yDiff > 0){
                            direction = 2;
                        } else {
                            direction = 0;
                        }
                    }

                    for(int i = 0; i < length - 1; i++){
                        moveList.Add(direction);
                    }

                    switch(direction){
                        case 0: posY--; animator.SetInteger("direction", 1); break;
                        case 1: posX++; animator.SetInteger("direction", 2); break;
                        case 2: posY++; animator.SetInteger("direction", 3); break;
                        case 3: posX--; animator.SetInteger("direction", 0); break;
                    }

                    StartCoroutine(MoveToSpot(MapManager.getPosition(posX, posY)));


                } else {
                    stepBack();
                }


            } else if(index == 2){
                // Ghost 3 AI
                List<int> validDir = new List<int>();
                for(int i = 0; i < 4; i++){
                    int newX = posX;
                    int newY = posY;

                    switch(i){
                        case 0: newY--; break;
                        case 1: newX++; break;
                        case 2: newY++; break;
                        case 3: newX--; break;
                    }

                    if(MapManager.isValidPosition(newX, newY) && !MapManager.isSpawnPosition(newX, newY) && !(newX == previousX && newY == previousY)){
                        validDir.Add(i);
                    }
                }

                if(validDir.Count > 0){
                    int rand = Random.Range(0, validDir.Count);
                    previousX = posX;
                    previousY = posY;
                    int direction = validDir[rand];

                    switch(direction){
                        case 0: posY--; animator.SetInteger("direction", 1); break;
                        case 1: posX++; animator.SetInteger("direction", 2); break;
                        case 2: posY++; animator.SetInteger("direction", 3); break;
                        case 3: posX--; animator.SetInteger("direction", 0); break;
                    }

                    StartCoroutine(MoveToSpot(MapManager.getPosition(posX, posY)));

                } else {
                    stepBack();
                }

            } else if(index == 3){
                // Ghost 4 AI
                string quadrant = MapManager.getQuadrant(posX, posY);
                int direction = 0;

                bool up = MapManager.isValidPosition(posX, posY - 1) && !MapManager.isSpawnPosition(posX, posY - 1);
                bool right = MapManager.isValidPosition(posX + 1, posY) && !MapManager.isSpawnPosition(posX + 1, posY);
                bool down = MapManager.isValidPosition(posX, posY + 1) && !MapManager.isSpawnPosition(posX, posY + 1);
                bool left = MapManager.isValidPosition(posX - 1, posY) && !MapManager.isSpawnPosition(posX - 1, posY);

                if(!inPosition){
                    if((posX == 1 && posY == 1) || (posX == 26 && posY == 1) || (posX == 26 && posY == 27) || (posX == 1 && posY == 27)){
                        inPosition = true;
                    }
                }
               
                switch(quadrant){
                    case "topLeft": {
                        if(inPosition){
                            if(up && right && left && down){
                                direction = 0;
                            } else if(up && left && down){
                                direction = 3;
                            } else if(up && left && right){
                                direction = 1;
                            } else if(right && up){
                                direction = 0;
                            } else if(right && down){
                                direction = 1;
                            } else if(left && down){
                                direction = 2;
                            } else direction = ghost4Direction;  
                        } else {
                            Debug.Log("Not in position");
                            if(up){
                                direction = 0;
                            } else if(left){
                                direction = 3;
                            } else if(down){
                                direction = 2;
                            } else if(right){
                                direction = 1;
                            }
                        }
                                         
                        break;
                    }
                    case "topRight": {
                        if(inPosition){
                            if(left && right && up){
                                direction = 0;
                            } else if(up && left && !right && !down){
                                direction = 3;
                            } else if(up && right && down && !left){
                                direction = 2;
                            } else if(down && right){
                                direction = 1;
                            } else if(down && left){
                                direction = 2;
                            } else direction = ghost4Direction;
                        } else {
                            if(up){
                                direction = 0;
                            } else if(right){
                                direction = 1;
                            } else if(down){
                                direction = 2;
                            } else if(left){
                                direction = 3;
                            }
                        }
                        break;
                    }
                    case "bottomRight": {
                        if(inPosition){
                            if(left && up && down && right){
                                direction = 2;
                            } else if(up && !left && down && right){
                                direction = 1;
                            } else if(left && down && !right && !up){
                                direction = 2;
                            } else if(up && left && !down && !right){
                                direction = 3;
                            } else if(up && right && !left && !down){
                                direction = 0;
                            } else if(!up && right && left && down){
                                direction = 3;
                            } else direction = ghost4Direction;
                        } else {
                            if(down){
                                direction = 2;
                            } else if(right){
                                direction = 1;
                            } else if(up){
                                direction = 0;
                            } else if(left){
                                direction = 3;
                            }
                        }
                        break;
                    }
                    case "bottomLeft": {
                        if(inPosition){
                            if(!up && left && right && down){
                                direction = 2;
                            } else if(up && left && !down && !right){
                                direction = 3;
                            } else if(up && right && !left && !down){
                                direction = 0;
                            } else if(!up && !left && right && down){
                                direction = 1;
                            } else if(up && left && down && !right){
                                direction = 0;
                            } else direction = ghost4Direction; 
                        } else {
                            if(down){
                                direction = 2;
                            } else if(left){
                                direction = 3;
                            } else if(up){
                                direction = 0;
                            } else if(right){
                                direction = 1;
                            }
                        }
                        break;
                    }
                }

                previousX = posX;
                previousY = posY;

                switch(direction){
                    case 0: posY--; animator.SetInteger("direction", 1); break;
                    case 1: posX++; animator.SetInteger("direction", 2); break;
                    case 2: posY++; animator.SetInteger("direction", 3); break;
                    case 3: posX--; animator.SetInteger("direction", 0); break;
                }

                ghost4Direction = direction;
                StartCoroutine(MoveToSpot(MapManager.getPosition(posX, posY)));

            }
        }
        
    }

    private void stepBack(){
        int tempX = previousX;
        previousX = posX;
        posX = tempX;

        int tempY = previousY;
        previousY = posY;
        posY = tempY;

        int xDiff = posX - previousX;
        int yDiff = posY - previousY;

        if(xDiff == -1){
            animator.SetInteger("direction", 0);
        } else if(xDiff == 1){
            animator.SetInteger("direction", 2);
        } else if(yDiff == -1){
            animator.SetInteger("direction", 1);
        } else if(yDiff == 1){
            animator.SetInteger("direction", 3);
        }

        StartCoroutine(MoveToSpot(MapManager.getPosition(posX, posY)));
    }

    public static void killedGhost(int ghostIndex){
        ghosts[ghostIndex].animator.SetTrigger("dead");
        ghosts[ghostIndex].ghostState = GhostState.Dead;
        ghostDeadCount++;
        ghostDead = true;
        ComponentManager.audioManager.changeMusicState(AudioManager.MusicState.Dead);
        ghosts[ghostIndex].StartCoroutine(ghostRebirth(ghostIndex));
    }

    static IEnumerator ghostRebirth(int ghostIndex){
        yield return new WaitForSeconds(5);
        
        ghostDeadCount--;
        if(ghostDeadCount == 0){
            ghostDead = false;
            if(staticGhostState == GhostState.Alive){
                ComponentManager.audioManager.changeMusicState(AudioManager.MusicState.Normal);
            } else {
                ComponentManager.audioManager.changeMusicState(AudioManager.MusicState.Scared);
            }
        }

        ghosts[ghostIndex].animator.SetTrigger("alive");
        ghosts[ghostIndex].ghostState = GhostState.Alive;

        yield return null;
    }

    public void updateGhostState(GhostState state){

        switch(state){
            case GhostState.Alive: {
                if(!ghostDead) ComponentManager.audioManager.changeMusicState(AudioManager.MusicState.Normal); 
                speed = baseSpeed;
                break;
            }
            case GhostState.Scared: {
                ComponentManager.audioManager.changeMusicState(AudioManager.MusicState.Scared); 
                timerCounter = 0;
                timer = 0.0f;
                staticGhostState = GhostState.Scared;
                timerActive = true;
                UIManager.ghostTimer = 10;
                break;
            }
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
                }
            }
        }
    }

    private void updateToScared(int i){
        ghosts[i].ghostState = GhostState.Scared;
        ghosts[i].animator.SetTrigger("scared");
        ghosts[i].speed = 3.0f;
        ghosts[i].inPosition = false;
    }
    private void updateToAlive(int i){
        ghosts[i].ghostState = GhostState.Alive;
        ghosts[i].animator.SetTrigger("alive");
        ghosts[i].speed = baseSpeed;
    }
    private void updateToDead(int i){
        ghosts[i].ghostState = GhostState.Dead;
        ghosts[i].animator.SetTrigger("dead");
        ghosts[i].speed = baseSpeed;
    }
    private void updateToRecovering(int i){
        ghosts[i].ghostState = GhostState.Recovering;
        ghosts[i].animator.SetTrigger("recovering");
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
        
        tweening = false;
        yield return null;
    }
}
