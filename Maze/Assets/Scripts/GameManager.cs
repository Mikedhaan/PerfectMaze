using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int sizeX, sizeY;
    private GameObject currentMaze;
    public MazeGenerator mazePrefab;
    private MazeGenerator mazeGenerator;
    public GameObject cameraPrefab;

	// Use this for initialization
	void Start () {
        //BeginGame();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BeginGame()
    {
        StopAllCoroutines();
        if (currentMaze != null)
            Destroy(currentMaze);

        if (sizeX != 0 && sizeY != 0)
        {
            mazeGenerator = Instantiate(mazePrefab) as MazeGenerator;
            mazeGenerator.name = "Maze";
            mazeGenerator.sizeX = sizeX;
            mazeGenerator.sizeY = sizeY;
            GameObject cameraGo = Instantiate(cameraPrefab);
            currentMaze = new GameObject("currentMaze");
            mazeGenerator.transform.parent = currentMaze.transform;
            cameraGo.transform.parent = currentMaze.transform;
        }
        else
        {
            Debug.LogError("Width or Height = 0");
        }
    }
    
    public void RestartGame()
    {
        Destroy(currentMaze);
        BeginGame();
    }

    public void SetXSize(string newText)
    {
        int x = int.Parse(newText);
        sizeX = x;   
    }

    public void SetYSize(string newText)
    {
        int y = int.Parse(newText);
        sizeY = y;
    }
}
