using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grape_Spawner : MonoBehaviour
{

    private int grapeCount = 5;

    public GameObject grape;
    public GameObject canvas;

    void Awake(){
        StartCoroutine(SpawnGrapes());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnGrapes() {
      
        for(int i = 0; i < grapeCount; i++){
            yield return new WaitForSeconds(0.25f);
            GameObject newGrape = Instantiate(grape);
            newGrape.transform.parent = canvas.transform;
        }


        yield return null;
    }
}
