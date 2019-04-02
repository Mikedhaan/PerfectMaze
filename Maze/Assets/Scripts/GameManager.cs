using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public MazeGenerator mazePrefab;
    private MazeGenerator mazeGenerator;
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
        StartCoroutine(mazeGenerator.CreateGrid());
    }
    
    void RestartGame()
    {
        Destroy(mazeGenerator.gameObject);
        BeginGame();
    }
}
