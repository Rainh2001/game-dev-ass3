using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween
{
    public Transform Target { get; private set; }
    public Vector3 StartPos { get; private set; }
    public Vector3 EndPos { get; private set; }
    public float StartTime { get; private set; }
    public float Duration { get; private set; }

    public Tween(Transform target, Vector3 start, Vector3 end, float startTime, float duration){
        this.Target = target;
        this.StartPos = start;
        this.EndPos = end;
        this.StartTime = startTime;
        this.Duration = duration;
    }

}
