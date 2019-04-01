using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public GameObject northWall;
    public GameObject southWall;
    public GameObject eastWall;
    public GameObject westWall;

    public Vector2 direction;
    public bool isVisited;

    private void Start()
    {
        this.isVisited = false;
    }
}

