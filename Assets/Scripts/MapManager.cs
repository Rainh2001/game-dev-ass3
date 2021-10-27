using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public static List<List<int>> map = new List<List<int>>();
    private static Vector3 mapStart;
    private static GameObject topLeftTile;
    public static float tileSize;

    public Vector3 center;

    void Awake(){
        topLeftTile = GameObject.FindGameObjectWithTag("FirstTile");
        tileSize = topLeftTile.GetComponent<SpriteRenderer>().bounds.size.x;
        mapStart = topLeftTile.transform.position;

        int[,] levelMap = {
            {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
            {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
            {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
            {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
            {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
            {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
            {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
            {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
            {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
            {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
            {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
            {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
            {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
        };

        // Add existing array (top-left quadrant)
        for(int i = 0; i < levelMap.GetLength(0); i++){
            List<int> row = new List<int>();
            for(int j = 0; j < levelMap.GetLength(1); j++){
                row.Add(levelMap[i, j]);
            }
            map.Add(row);
        }

        // Mirror horizontally (top-right quadrant)
        for(int i = 0; i < levelMap.GetLength(0); i++){
            for(int j = levelMap.GetLength(1)-1; j >= 0; j--){
                map[i].Add(levelMap[i, j]);
            }
        }

        // Mirror vertically (bottom-left quadrant)
        // Mirror horizontally and vertically (bottom-right quadrant)
        for(int i = map.Count-2; i >= 0; i--){
            List<int> row = new List<int>();
            for(int j = 0; j < map[i].Count; j++){
                row.Add(map[i][j]);
            }
            map.Add(row);
        }

        center = getPosition(map[0].Count/2, map.Count/2);
    }

    public static Vector3 getPosition(int x, int y){
        return new Vector3(mapStart.x + tileSize * x, mapStart.y + tileSize * y * -1, 0);
    }

    public static Vector3 getPosition(float x, float y){
        return new Vector3(mapStart.x + tileSize * x, mapStart.y + tileSize * y * -1, 0);
    }

    public static bool isValidPosition(int x, int y){
        if(x < 0 || y < 0 || x >= map[0].Count || y >= map.Count) return false;

        int tile = map[y][x];
        if(tile != 0 && tile != 5 && tile != 6) return false;

        return true;
    }

    public static bool isPellet(int x, int y){
        int tile = map[y][x];

        if(tile == 0) return false;

        map[y][x] = 0;
        return true;
    }
}
