using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape_Spawner : MonoBehaviour
{

    private int grapeCount = 5;

    public GameObject grape;

    void Awake(){
        StartCoroutine(SpawnGrape());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnGrape() {

        for(int i = 0; i < grapeCount; i++){
            yield return new WaitForSeconds(0.25f);
            Instantiate(grape);
        }


        yield return null;
    }
}
