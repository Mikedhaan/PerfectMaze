using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

    public int sizeX, sizeY;
    Cell[,] Cells;
    List<Cell> activeCells;
    public float generateDelay;
    public Cell cell;
    public GameObject gridWall;
    public int failedTries;

    private Vector2[] directions =
        {
            new Vector2 (0, 1),
            new Vector2(1, 0),
            new Vector2(0,-1),
            new Vector2(-1, 0)
        };

    public List<Cell> allCells;

    public Vector2 nextDirection;
    public Vector2 currentCell;
    public Vector2 nextCell;
    public int randomDirection;


    // Use this for initialization
    void Start()
    {
        Cells = new Cell[sizeX, sizeY];
        allCells = new List<Cell>();
        currentCell = new Vector2(5, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (failedTries > 20)
        {
            FinishMaze();
        }
    }

    void RandomNextStep()
    {
        randomDirection = Random.Range(0, 4);
        nextDirection = directions[randomDirection];
        nextCell = currentCell + nextDirection;
    }

    public IEnumerator GenerateGrid()
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
        while (true)
        {
            RandomNextStep();
            failedTries++;
            if (nextCell.x < sizeX && nextCell.x >= 0 && nextCell.y < sizeY && nextCell.y >= 0)
            {

                if (!Cells[(int)nextCell.x, (int)nextCell.y].isVisited)
                {                  
                    RemoveWalls();
                    Cells[(int)nextCell.x, (int)nextCell.y].isVisited = true;
                    failedTries = 0;
                }

            }
                
            yield return new WaitForSeconds(generateDelay);
        }
    }

    void checkBounds()
    {
        if (nextCell.x < sizeX && nextCell.x >= 0 && nextCell.y < sizeY && nextCell.y >= 0)
        {

        }
        else
            RandomNextStep();
    }

    void FinishMaze()
    {
        failedTries = 0;
        StopCoroutine(MazeMaker());
        foreach(Cell cell in allCells)
        {
            if (!cell.isVisited)
            {
                currentCell = new Vector2((int)cell.transform.position.x, (int)cell.transform.position.y);
                Cells[(int)currentCell.x, (int)currentCell.y].isVisited = true;
                RandomNextStep();

                    RemoveWalls();
                
                Debug.Log((int)currentCell.x + " " + (int)currentCell.y);
                StartCoroutine(MazeMaker());
                return;
            }
        }
    }

 

    Cell getCell(Vector2 cellPos)
    {
        return Cells[(int)cellPos.x, (int)cellPos.y];
    }

    void RemoveWalls()
    {
        if (randomDirection == 0)
        {
            //up
            getCell(currentCell).GetComponent<Cell>().northWall.SetActive(false);
            getCell(nextCell).GetComponent<Cell>().southWall.SetActive(false);
        }
        if (randomDirection == 1)
        {
            //right
            getCell(currentCell).GetComponent<Cell>().eastWall.SetActive(false);
            getCell(nextCell).GetComponent<Cell>().westWall.SetActive(false);
        }
        if (randomDirection == 2)
        {
            //down
            getCell(currentCell).GetComponent<Cell>().southWall.SetActive(false);
            getCell(nextCell).GetComponent<Cell>().northWall.SetActive(false);
        }
        if (randomDirection == 3)
        {
            //left
            getCell(currentCell).GetComponent<Cell>().westWall.SetActive(false);
            getCell(nextCell).GetComponent<Cell>().eastWall.SetActive(false);
        }

        currentCell = nextCell;
    }

}