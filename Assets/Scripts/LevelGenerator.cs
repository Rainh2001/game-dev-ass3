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

                int topRight = (i == 0 || j == map[0].Count-1) ? 0 : map[i-1][j+1];
                int topLeft = (i == 0 || j == 0) ? 0 : map[i-1][j-1];
                int bottomRight = (i == map.Count-1 || j == map[0].Count-1) ? 0 : map[i+1][j+1];
                int bottomLeft = (i == map.Count-1 || j == 0) ? 0 : map[i+1][j-1];

                int top = i == 0 ? 0 : map[i-1][j];
                int left = j == 0 ? 0 : map[i][j-1];
                int bottom = i == map.Count-1 ? 0 : map[i+1][j];
                int right = j == map[0].Count-1 ? 0 : map[i][j+1];

                switch(tileNum){
                    case 1: // Outside Corner
                        if((top == 2 || top == 7 || top == 1) && (left == 2 || left == 7 || left == 1)){
                            rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                        } else if((top == 2 || top == 7 || top == 1) && (right == 2 || right == 7 || right == 1)){
                            rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                        } else if((bottom == 2 || bottom == 7 || bottom == 1) && (right == 2 || right == 7 || right == 1)){
                            rotation = Quaternion.identity;
                        } else if((bottom == 2 || bottom == 7 || bottom == 1) && (left == 2 || left == 7 || left == 1)){
                            rotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
                        }
                        break;
                    case 2: // Outside Wall
                        bool topConnection = (top == 2 || top == 7 || top == 1);
                        bool leftConnection = (left == 2 || left == 7 || left == 1);
                        bool rightConnection = (right == 2 || right == 7 || right == 1);
                        bool bottomConnection = (bottom == 2 || bottom == 7 || bottom == 1);
                        
                        // int connections = (topConnection ? 1 : 0) + (leftConnection ? 1 : 0) + (rightConnection ? 1 : 0) + (bottomConnection ? 1 : 0);

                        if((topConnection || bottomConnection) && (left == 0 || right == 5 || right == 6)){
                            rotation = Quaternion.identity;
                        } else if((topConnection || bottomConnection) && (right == 0 || left == 5 || left == 6)){
                            rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f); 
                        } else if((leftConnection || rightConnection) && (top == 0 || bottom == 5 || bottom == 6)){
                            rotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
                        } else if((leftConnection || rightConnection) && (bottom == 0 || top == 5 || top == 6)){
                            rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                        }
                        break;

                    case 3: // Inside Corner
                        topConnection = (top == 3 || top == 4 || top == 7);
                        leftConnection = (left == 3 || left == 4 || left == 7);
                        rightConnection = (right == 3 || right == 4 || right == 7);
                        bottomConnection = (bottom == 3 || bottom == 4 || bottom == 7);

                        int connections = (topConnection ? 1 : 0) + (leftConnection ? 1 : 0) + (rightConnection ? 1 : 0) + (bottomConnection ? 1 : 0);

                        if(connections <= 2){
                            if(topConnection && rightConnection 
                            && (bottomLeft == 5 || bottomLeft == 6 || bottomLeft == 0 || topRight == 5 || topRight == 6 || topRight == 0)){
                                rotation = Quaternion.identity;
                            } else if(topConnection && leftConnection
                            && (bottomRight == 5 || bottomRight == 6 || bottomRight == 0 || topLeft == 5 || topLeft == 6 || topLeft == 0)){
                                rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f); 
                            } else if(leftConnection && bottomConnection
                            && (topRight == 5 || topRight == 6 || topRight == 0 || bottomLeft == 5 || bottomLeft == 6 || bottomLeft == 0)){
                                rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                            } else if(bottomConnection && rightConnection
                            && (bottomRight == 5 || bottomRight == 6 || bottomRight == 0 || topLeft == 5 || topLeft == 6 || topLeft == 0)){
                                rotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
                            }
                        } else {
                            if(rightConnection && topConnection && (topRight == 0 || topRight == 5 || topRight == 6)){
                                rotation = Quaternion.identity;
                                break;
                            } else if(rightConnection && bottomConnection && (bottomRight == 0 || bottomRight == 5 || bottomRight == 6)){
                                rotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
                                break;
                            } else if(leftConnection && bottomConnection && (bottomLeft == 0 || bottomLeft == 5 || bottomLeft == 6)){
                                rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                                break;
                            } else if(leftConnection && topConnection && (topLeft == 0 || topLeft == 5 || topLeft == 6)){
                                rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f); 
                                break;
                            }
                        }
                        
                        break;
                    case 4: // Inside Wall

                        topConnection = (top == 3 || top == 4 || top == 7);
                        leftConnection = (left == 3 || left == 4 || left == 7);
                        rightConnection = (right == 3 || right == 4 || right == 7);
                        bottomConnection = (bottom == 3 || bottom == 4 || bottom == 7);
                        
                        connections = (topConnection ? 1 : 0) + (leftConnection ? 1 : 0) + (rightConnection ? 1 : 0) + (bottomConnection ? 1 : 0);

                        if(connections == 1){
                            if(topConnection || bottomConnection) {rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f); break;}
                            if(leftConnection || rightConnection) {rotation = Quaternion.identity; break;}
                        } else if(connections == 2){
                            if(topConnection && bottomConnection) { rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f); break; }
                            if(leftConnection && rightConnection) { rotation = Quaternion.identity; break; }
                        } else {
                            if(topConnection && bottomConnection && rightConnection){
                                rotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
                                break;
                            }else if(topConnection && bottomConnection && leftConnection){
                                rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                                break;
                            }
                        }
                        break;


                    case 7: // Intersection
                        topConnection = (top == 1 || top == 2 || top == 7);
                        leftConnection = (left == 1 || left == 2 || left == 7);
                        rightConnection = (right == 1 || right == 2 || right == 7);
                        bottomConnection = (bottom == 1 || bottom == 2 || bottom == 7);

                        int quadrant = 0;

                        if(i < map.Count/2){
                            if(j > (map[0].Count-1)/2){
                                quadrant = 0;
                            } else {
                                quadrant = 1;
                            }

                        } else {

                            if(j > (map[0].Count-1)/2){
                                quadrant = 4;
                            } else {
                                quadrant = 3;
                            }
                        }

                        if(leftConnection && rightConnection){

                            if(top == 0){
                                if(quadrant == 0){
                                    rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                                } else {
                                    rotation = Quaternion.identity;
                                }
                                break;
                            } else {
                                if(quadrant == 3){
                                    rotation = Quaternion.Euler(0.0f, 180.0f, 180.0f);
                                } else {
                                    rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                                }
                                break;
                            }

                        } else if(bottomConnection && topConnection){

                            if(left == 0){
                                if(quadrant == 1){
                                    rotation = Quaternion.Euler(180.0f, 0.0f, 90.0f);
                                } else {
                                    rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                                }
                                break;
                            } else {
                                if(quadrant == 4){
                                    rotation = Quaternion.Euler(180.0f, 0.0f, 270.0f);
                                } else {
                                    rotation = Quaternion.Euler(0.0f, 0.0f, 270.0f);
                                }
                                break;
                            }

                        }

                        break;
                }

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
