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
    private AudioSource audioSource;
    public AudioClip movingNoEating;
    public AudioClip movingAndEating;
    public AudioClip death;
    public ParticleSystem particle;
    public ParticleSystem wallCollision;
    public ParticleSystem deathParticle;
    private AudioSource wallCollisionAudio;

    private int pelletsEaten;
    private int totalPellets;

    private bool collided = false;

    private bool teleporting = false;
    private GhostController ghostController;

    private enum PacState { Alive, Dead }
    private PacState pacState = PacState.Alive;

    void Awake(){
        ComponentManager.pacStudentController = this;
        pelletsEaten = 0;
        GameObject[] pelletGO = GameObject.FindGameObjectsWithTag("Pellet");
        GameObject[] powerGO = GameObject.FindGameObjectsWithTag("Power_Pellet");
        totalPellets = pelletGO.Length + powerGO.Length;

        animator = gameObject.GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = movingAndEating;
        animator.enabled = false;
        wallCollisionAudio = GameObject.FindWithTag("WallCollision").GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(pelletsEaten == totalPellets){
            UIManager.gameOver = true;
        }

        if(UIManager.gameOver){
            audioSource.loop = false;
        }

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

        if(!tweening && initialized && !teleporting && pacState == PacState.Alive && !UIManager.countingDown){
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

                if(MapManager.isPellet(newX, newY)){
                    audioSource.clip = movingAndEating;
                } else {
                    audioSource.clip = movingNoEating;
                }

                if(!audioSource.isPlaying){
                    audioSource.Play();
                }

                if(!playing){
                    animator.enabled = true;
                    particle.Play();
                }

                currentInput = lastInput;
                posX = newX;
                posY = newY;
                animator.SetInteger("direction", direction);
                StartCoroutine(MoveToSpot(MapManager.getPosition(newX, newY)));
                playing = true;
                collided = false;
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
                    if(MapManager.isPellet(newX, newY)){
                        audioSource.clip = movingAndEating;
                    } else {
                        audioSource.clip = movingNoEating;
                    }

                    if(!audioSource.isPlaying){
                        audioSource.Play();
                    }

                    if(!playing){
                        animator.enabled = true;
                        particle.Play();
                    }

                    posX = newX;
                    posY = newY;
                    animator.SetInteger("direction", direction);
                    StartCoroutine(MoveToSpot(MapManager.getPosition(newX, newY)));
                    playing = true;
                    collided = false;
                }
            }

            if(!playing){
                particle.Stop();
                animator.enabled = false;
                audioSource.Stop();
                if(!collided && !teleporting) {
                    wallCollision.Play();
                    wallCollisionAudio.Play();
                }
                collided = true;
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
            if(teleporting || pacState == PacState.Dead) break;
            t = (Time.time - startTime)/duration;
            transform.position = Vector3.Lerp(startPos, position, t);
            yield return null;
        }

        if(!teleporting && pacState != PacState.Dead){
            particle.Play();
            transform.position = position;
        }
        // Debug.Log(posX + " " + posY);
        tweening = false;
        yield return null;
    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Pellet"){
            UIManager.score += 10;
            pelletsEaten++;
            Destroy(other.gameObject);
        } else if(other.tag == "BonusCherry"){
            UIManager.score += 100;
            CherryController.destroyed = true;
            Destroy(other.gameObject);
        } else if (other.tag == "TeleportLeft" || other.tag == "TeleportRight"){
            teleporting = true;
        } else if(other.tag.Substring(0, 5) == "Enemy"){

            // Check if the enemy is in Alive or Scared/Recovering
            GhostController ghost = other.gameObject.GetComponent<GhostController>();
            if(ghost.ghostState == GhostController.GhostState.Alive){

                if(pacState == PacState.Alive){
                    ComponentManager.uIManager.loseLife();
                    pacState = PacState.Dead;
                    animator.SetTrigger("death");
                    // audioSource.Stop();
                    audioSource.clip = death;
                    audioSource.volume = 0.5f;
                    audioSource.Play();
                    particle.Stop();
                    deathParticle.Play();
                    Invoke("playerDeath", 2.3f);
                }

            } else if(ghost.ghostState == GhostController.GhostState.Scared || ghost.ghostState == GhostController.GhostState.Recovering){
                
                if(pacState == PacState.Alive){
                    UIManager.score += 300;
                    int index = int.Parse(other.tag[other.tag.Length-1] + "") - 1;
                    GhostController.killedGhost(index);
                }
                
            }

        } else if(other.tag == "Power_Pellet"){
            ComponentManager.ghostController.updateGhostState(GhostController.GhostState.Scared);
            pelletsEaten++;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.tag == "TeleportLeft"){
            posX = 26;
            posY = 14;
            Vector3 newPosition = MapManager.getPosition(posX, posY);
            transform.position = newPosition;
        } else if(other.tag == "TeleportRight"){
            posX = 1;
            posY = 14;
            Vector3 newPosition = MapManager.getPosition(posX, posY);
            transform.position = newPosition;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "TeleportLeft" || other.tag == "TeleportRight") teleporting = false;    
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Wall") Debug.Log("Wall collision");
    }

    void playerDeath(){
        audioSource.loop = true;
        audioSource.clip = movingNoEating;
        initialized = false;
        posX = 1;
        posY = 1;
        transform.position = MapManager.getPosition(1, 1);
        transform.rotation = Quaternion.identity;
        animator.SetTrigger("alive");
        pacState = PacState.Alive;
    }
}
