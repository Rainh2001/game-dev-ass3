using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Tweener : MonoBehaviour
{

    private Tween activeTween;

    private float xDistance = 6.4f;
    
    private float yDistance = 5.12f;
    private float speed = 3.55f;
    private int tweenCount = 0;


    void Awake()
    {
        Vector3 destination = new Vector3(transform.position.x + xDistance, transform.position.y, transform.position.z);
        activeTween = new Tween(transform, transform.position, destination, Time.time, xDistance/speed);
    }

    // Update is called once per frame
    void Update()
    {

        float distance = Vector3.Distance(activeTween.EndPos, transform.position);

        if(distance > 0.1f){

                float t = (Time.time - activeTween.StartTime) / activeTween.Duration;

                transform.position = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, t);


        } else {
            transform.position = activeTween.EndPos;
            tweenCount++;
            if(tweenCount == 4) tweenCount = 0;

            Vector3 destination = transform.position;
            float duration = 0.0f;

            switch(tweenCount){
                case 0:
                    destination = new Vector3(transform.position.x + xDistance, transform.position.y, transform.position.z);
                    duration = xDistance/speed;
                    break;
                case 1:
                    destination = new Vector3(transform.position.x, transform.position.y - yDistance, transform.position.z);
                    duration = yDistance/speed;
                    break;
                case 2:
                    destination = new Vector3(transform.position.x - xDistance, transform.position.y, transform.position.z);
                    duration = xDistance/speed;
                    break;
                case 3:
                    destination = new Vector3(transform.position.x, transform.position.y + yDistance, transform.position.z);
                    duration = yDistance/speed;
                    break;
            }

            activeTween = new Tween(transform, transform.position, destination, Time.time, duration);

        }

        // for(int i = activeTweens.Count - 1; i >= 0; i--){
            
        //     float distance = Vector3.Distance(activeTweens[i].EndPos, activeTweens[i].Target.position);

        //     if(distance > 0.1f){

        //         float t = (Time.time - activeTweens[i].StartTime) / activeTweens[i].Duration;

        //         activeTweens[i].Target.position = Vector3.Lerp(activeTweens[i].StartPos, activeTweens[i].EndPos, t);
                

        //     } else {
        //         activeTweens[i].Target.position = activeTweens[i].EndPos;
        //         activeTweens.RemoveAt(i);
        //     }

        // }

    }

    // public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration){

    //     if(!TweenExists(targetObject)){
    //         Tween tween = new Tween(targetObject, startPos, endPos, Time.time, duration);
    //         activeTweens.Add(tween);
    //         return true;
    //     } else {
    //         return false;
    //     }

    // }

    // public bool TweenExists(Transform target){
    //     foreach(Tween tween in activeTweens){
    //         if(GameObject.ReferenceEquals(tween.Target, target)) return true;
    //     }
    //     return false;
    // }

}
