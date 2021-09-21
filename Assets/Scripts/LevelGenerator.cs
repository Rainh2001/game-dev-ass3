using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public GameObject tileOutsideCorner;
    public GameObject tileOutsideWall;
    public GameObject tileInsideCorner;
    public GameObject tileInsideWall;
    public GameObject tileIntersection;
    public GameObject tilePellet;
    public GameObject tilePowerPellet;
    public GameObject player;

    private List<List<int>> map = new List<List<int>>();

    void Awake(){

        float size = 0.0f;

        // Remove all children from the level
        foreach(Transform child in transform){
            size = child.GetComponent<SpriteRenderer>().bounds.size.x;
            Destroy(child.gameObject);
        }

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

        // Convert initial array from one quadrant into four quadrants and assign to map variable
            // Add existing array (top-left quadrant)
            // Mirror horizontally (top-right quadrant)
            // Mirror vertically (bottom-left quadrant)
            // Mirror horizontally and vertically (bottom-right quadrant)

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


        // for(int i = 0; i < map.Count; i++){
        //     string row = "";
        //     for(int j = 0; j < map[i].Count; j++){
        //         row += map[i][j].ToString();
        //     }
        //     Debug.Log(row);
        // }

        
        // Iterate through map variable and instantiate the expected tile with the intended rotation
            // Based on position in the matrix, figure out the correct positioning for the tile
            // Analyze horizontal and vertical adjacent tiles in the list to figure out rotation
            
        for(int i = 0; i < map.Count; i++){
            for(int j = 0; j < map[i].Count; j++){

                int tileNum = map[i][j];
                if(tileNum == 0) continue;

                // Get the actual tile GameObject
                GameObject tile = getTile(tileNum);
                

                // Based on position in the matrix, figure out the correct positioning for the tile
                Vector3 position = getPosition(size, i, j);


                // Analyze horizontal and vertical adjacent tiles in the list to figure out rotation
                Quaternion rotation = Quaternion.identity;


                // Instantiate the tile at the given position with the calculate rotation
                Instantiate(tile, position, rotation, transform);

            }
        }

        // Instantiate the Player at the map[1][1] position (just inside from the top-left corner)
        Instantiate(player, getPosition(size, 1, 1), Quaternion.identity, transform);

    }

    private GameObject getTile(int num){

        switch(num){
            case 1: return tileOutsideCorner;
            case 2: return tileOutsideWall;
            case 3: return tileInsideCorner;
            case 4: return tileInsideWall;
            case 5: return tilePellet;
            case 6: return tilePowerPellet;
            case 7: return tileIntersection;
        }

        return null;
    }

    private Vector3 getPosition(float size, int i, int j){
        return new Vector3(size * j, size * i * -1, 0);
    }
}
