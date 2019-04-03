using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

    private float sizeX,sizeY;
    private MazeGenerator mazeGenerator;
    private Camera mazeCamera;

	// Use this for initialization
	void Start () {
        mazeCamera = this.gameObject.GetComponent<Camera>();
        mazeGenerator = GameObject.Find("Maze").GetComponent<MazeGenerator>();
        sizeX = mazeGenerator.sizeX;
        sizeY = mazeGenerator.sizeY;


        mazeCamera.transform.position = new Vector3(sizeX / 2, (sizeY / 2)-0.5f, -10);


        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = sizeX / sizeY;

        if (screenRatio >= targetRatio)
        {
            mazeCamera.orthographicSize = sizeY / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            mazeCamera.orthographicSize = sizeY / 2 * differenceInSize;
        }
        mazeCamera.orthographicSize += 1f;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
