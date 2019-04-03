using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int sizeX, sizeY;

    public MazeGenerator mazePrefab;
    private MazeGenerator mazeGenerator;
    public GameObject cameraPrefab;

	// Use this for initialization
	void Start () {
        BeginGame();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void BeginGame()
    {
        mazeGenerator = Instantiate(mazePrefab) as MazeGenerator;
        mazeGenerator.name = "Maze";
        mazeGenerator.sizeX = sizeX;
        mazeGenerator.sizeY = sizeY;
        Instantiate(cameraPrefab);
    }
    
    void RestartGame()
    {
        Destroy(mazeGenerator.gameObject);
        BeginGame();
    }
}
