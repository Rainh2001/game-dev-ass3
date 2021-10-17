using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape_Tweener : MonoBehaviour
{

    private int tweenNum = 0;
    private bool tweening = false;

    void Awake(){
        transform.position = new Vector3(-3.7f, 3.8f, 1.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!tweening){
            switch(tweenNum){
                case 0:
                    tweenNum = 1;
                    StartCoroutine(MoveToSpot(new Vector3(3.7f, 3.8f, 1.0f)));
                    break;
                case 1:
                    tweenNum = 2;
                    StartCoroutine(MoveToSpot(new Vector3(3.7f, 1.1f, 1.0f)));
                    break;
                case 2:
                    tweenNum = 3;
                    StartCoroutine(MoveToSpot(new Vector3(-3.7f, 1.1f, 1.0f)));
                    break;
                case 3:
                    tweenNum = 0;
                    StartCoroutine(MoveToSpot(new Vector3(-3.7f, 3.8f, 1.0f)));
                    break;
            }
        }
    }

    IEnumerator MoveToSpot(Vector3 position) {
        float startTime = Time.time;
        float duration = Vector3.Distance(transform.position, position)/5;
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
