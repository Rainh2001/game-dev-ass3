using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public static bool destroyed = false;
    private float timer;
    private int lastTime = 0;

    private float cherrySpeed = 6.5f;

    private enum CherryPosition { top, right, bottom, left }

    public GameObject cherryPrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer - lastTime >= 1){
            lastTime++;

            
            if(lastTime % 10 == 0){
                spawnCherry();
            }


        }
    }

    void spawnCherry(){
        CherryPosition pos = (CherryPosition)Random.Range(0, 4);
        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.zero;
        Vector3 cameraPos = Camera.main.transform.position;
        float tileSize = MapManager.tileSize;

        Camera camera = Camera.main;
        float halfHeight = camera.orthographicSize;
        float halfWidth = camera.aspect * halfHeight;

        Vector3 topLeft = new Vector3(cameraPos.x - halfWidth - tileSize/2, cameraPos.y + halfHeight + tileSize/2, 0);
        Vector3 topRight = new Vector3(cameraPos.x + halfWidth + tileSize/2, cameraPos.y + halfHeight + tileSize/2, 0);

        Vector3 bottomRight = new Vector3(cameraPos.x + halfWidth + tileSize/2, cameraPos.y - halfHeight - tileSize/2, 0);
        Vector3 bottomLeft = new Vector3(cameraPos.x - halfWidth - tileSize/2, cameraPos.y - halfHeight - tileSize/2, 0);

        switch(pos){
            case CherryPosition.top: 
                start = topLeft;
                end = topRight;
                break;
            case CherryPosition.right: 
                start = topRight;
                end = bottomRight;
                break;
            case CherryPosition.bottom: 
                start = bottomRight;
                end = bottomLeft;
                break;
            case CherryPosition.left: 
                start = bottomLeft;
                end = topLeft;
                break;
        }

        Vector3 instantiatePos = GetRandomVector3Between(start, end);

        Vector3 destroyPos = new Vector3(instantiatePos.x, instantiatePos.y, instantiatePos.z);

        switch(pos){
            case CherryPosition.top: 
                destroyPos.y = instantiatePos.y - (halfHeight*2 + tileSize);
                destroyPos.x = bottomRight.x - Mathf.Abs(destroyPos.x - bottomLeft.x);
                break;
            case CherryPosition.right: 
                destroyPos.x = instantiatePos.x - (halfWidth*2 + tileSize);
                destroyPos.y = bottomLeft.y + Mathf.Abs(destroyPos.y - topLeft.y);
                break;
            case CherryPosition.bottom: 
                destroyPos.y = instantiatePos.y + (halfHeight*2 + tileSize);
                destroyPos.x = topRight.x - Mathf.Abs(destroyPos.x - topLeft.x);
                break;
            case CherryPosition.left: 
                destroyPos.x = instantiatePos.x + (halfWidth*2 + tileSize);
                destroyPos.y = bottomRight.y + Mathf.Abs(destroyPos.y - topRight.y);
                break;
        }

        GameObject newCherry = Instantiate(cherryPrefab, instantiatePos, Quaternion.identity);
        StartCoroutine(MoveToSpot(newCherry.transform, destroyPos));

    }

    public Vector3 GetRandomVector3Between (Vector3 min, Vector3 max){
		return min + Random.Range(0.0f, 1.0f) * (max - min);
	}

    IEnumerator MoveToSpot(Transform target, Vector3 position) {
        float startTime = Time.time;
        float duration = Vector3.Distance(target.position, position)/cherrySpeed;
        float t = 0.0f;

        Vector3 startPos = target.position;

        while (t < 1.0f && !destroyed){
            t = (Time.time - startTime)/duration;
            target.position = Vector3.Lerp(startPos, position, t);
            yield return null;
        }

        if(!destroyed){
            target.position = position;
            Destroy(target.gameObject);
        }

        destroyed = false;
        yield return null;
    }
}
