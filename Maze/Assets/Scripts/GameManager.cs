using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int sizeX, sizeY;

    public bool playing = false;
    public float playTime = 0;

    public MazeGenerator mazePrefab;

    public GameObject cameraPrefab;
    public GameObject playerPrefab;

    public GameObject playMazeButton;
    public Text timerText;

    private GameObject currentMaze;

    private MazeGenerator mazeGenerator;


    // Use this for initialization
    void Start () {
        playMazeButton.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (mazeGenerator != null)
        {
            if (!playMazeButton.activeSelf && mazeGenerator.mazePlayable)
            {
                playMazeButton.SetActive(true);
            }
        }

        if(playing)
        {
            playTime += Time.deltaTime;
        }


        timerText.text = playTime.ToString("00.00"); //+ ":" + seconds.ToString("00");
    }

    public void BeginGame()
    {
        
        //
        StopAllCoroutines();

        if (currentMaze != null)
            Destroy(currentMaze);

        if (sizeX != 0 && sizeY != 0)
        {
            mazeGenerator = Instantiate(mazePrefab) as MazeGenerator;
            mazeGenerator.name = "Maze";
            mazeGenerator.sizeX = sizeX;
            mazeGenerator.sizeY = sizeY;

            //Instantiate new maze with parent so we can remove old maze upon new creation
            GameObject cameraGo = Instantiate(cameraPrefab);
            currentMaze = new GameObject("currentMaze");
            mazeGenerator.transform.parent = currentMaze.transform;
            cameraGo.transform.parent = currentMaze.transform;
        }
        else
        {
            Debug.LogError("Width or Height = 0");
        }

        //Turn off button on creation so it has a new reference
        playMazeButton.SetActive(false);
        playing = false;
        playTime = 0;
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

    public void spawnPlayer()
    {
        GameObject go = Instantiate(playerPrefab, new Vector3(0f, sizeY - 1f, -1f),Quaternion.identity);
        go.name = "Player";
        playing = true;
    }
}
