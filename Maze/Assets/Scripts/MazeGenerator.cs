﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

    public int sizeX, sizeY;
    public float generateDelay;

    public Cell cell;

    private Cell[,] Cells;

    private int failedTries;

    private Vector2[] directions =
        {
            new Vector2 (0, 1),
            new Vector2(1, 0),
            new Vector2(0,-1),
            new Vector2(-1, 0)
        };

    private List<Cell> allCells;

    private Vector2 nextDirection;
    private Vector2 currentCell;
    private Vector2 nextCell;
    private int randomDirection;


    // Use this for initialization
    void Start()
    {
        Cells = new Cell[sizeX, sizeY];
        allCells = new List<Cell>();
        currentCell = new Vector2((int)sizeX / 2, (int)sizeY / 2);

    }

    // Update is called once per frame
    void Update()
    {
        //if (failedTries >= 20)
        //{
        //    FinishMaze();
        //}
    }

    void RandomNextStep()
    {
        randomDirection = Random.Range(0, 4);
        nextDirection = directions[randomDirection];
        nextCell = currentCell + nextDirection;

        CheckBounds();
    }

    public IEnumerator CreateGrid()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                yield return new WaitForSeconds(generateDelay);
                CreateCell(x, y);
            }
        }
        StartCoroutine(MazeMaker());
    }

    void CreateCell(int posX, int posY)
    {
        Cell newCell = Instantiate(cell) as Cell;
        Cells[posX, posY] = newCell;
        newCell.transform.localPosition = new Vector3(posX, posY, 0);
        newCell.name = "Floor " + posX + " " + posY;
        newCell.transform.parent = this.transform;
        allCells.Add(newCell);
    }

    public IEnumerator MazeMaker()
    {
        for (failedTries = 0; failedTries < 20; failedTries++)
        {
            RandomNextStep();
            failedTries++;

            if (Cells[(int)nextCell.x, (int)nextCell.y].direction == new Vector2(0, 0))
            {
                RemoveWalls();

                Cells[(int)currentCell.x, (int)currentCell.y].direction = nextDirection;
                failedTries = 0;
                allCells.Remove(Cells[(int)currentCell.x, (int)currentCell.y]);

            }
            yield return new WaitForSeconds(generateDelay);

        }
        if (failedTries >= 20)
        {
            StartCoroutine(FinishMaze());
        }
    }

    void CheckBounds()
    {
        if (nextCell.x < sizeX && nextCell.x >= 0 && nextCell.y < sizeY && nextCell.y >= 0)
        {
            return;
        }
        else
            RandomNextStep();
    }

    IEnumerator FinishMaze()
    {

        foreach (Cell cell in allCells)
        {
            if (cell.direction == new Vector2(0, 0))
            {
                currentCell = new Vector2((int)cell.transform.position.x, (int)cell.transform.position.y);
                RandomNextStep();
                if (Cells[(int)nextCell.x, (int)nextCell.y].direction != new Vector2(0, 0))
                {
                    RemoveWalls();
                    cell.direction = nextDirection;
                    yield return new WaitForSeconds(generateDelay);

                }
            }

        }
        //cleanList();
    }

    void cleanList()
    {
        for (int i = allCells.Count; i > 0; i--)
        {
            if (allCells[i].direction != new Vector2(0, 0))
            {
                allCells.RemoveAt(i);
            }
        }

    }


    Cell GetCell(Vector2 cellPos)
    {
        return Cells[(int)cellPos.x, (int)cellPos.y];
    }

    void RemoveWalls()
    {
        if (randomDirection == 0)
        {
            //up
            GetCell(currentCell).GetComponent<Cell>().northWall.SetActive(false);
            GetCell(nextCell).GetComponent<Cell>().southWall.SetActive(false);
        }
        if (randomDirection == 1)
        {
            //right
            GetCell(currentCell).GetComponent<Cell>().eastWall.SetActive(false);
            GetCell(nextCell).GetComponent<Cell>().westWall.SetActive(false);
        }
        if (randomDirection == 2)
        {
            //down
            GetCell(currentCell).GetComponent<Cell>().southWall.SetActive(false);
            GetCell(nextCell).GetComponent<Cell>().northWall.SetActive(false);
        }
        if (randomDirection == 3)
        {
            //left
            GetCell(currentCell).GetComponent<Cell>().westWall.SetActive(false);
            GetCell(nextCell).GetComponent<Cell>().eastWall.SetActive(false);
        }

        currentCell = nextCell;
    }

}