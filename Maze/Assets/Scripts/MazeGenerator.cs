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
        currentCell = new Vector2(12, 12);
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
        checkBounds();
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
        while (failedTries <= 20)
        {
            RandomNextStep();
            failedTries++;

            Debug.Log("active");

            if (Cells[(int)nextCell.x, (int)nextCell.y].direction == new Vector2(0, 0))
            {
                RemoveWalls();

                Cells[(int)currentCell.x, (int)currentCell.y].direction = nextDirection;
                failedTries = 0;
            }

            yield return new WaitForSeconds(generateDelay);
        }
    }

    void checkBounds()
    {
        if (nextCell.x < sizeX && nextCell.x >= 0 && nextCell.y < sizeY && nextCell.y >= 0)
        {
            return;
        }
        else
            RandomNextStep();
    }

    void FinishMaze()
    {

        StopCoroutine(MazeMaker());
        foreach (Cell cell in allCells)
        {

            if (cell.direction == new Vector2(0, 0))
            {
                //Debug.Log((int)cell.transform.position.x +" "+ (int)cell.transform.position.y);
                currentCell = new Vector2((int)cell.transform.position.x, (int)cell.transform.position.y);

                RandomNextStep();

                if (Cells[(int)nextCell.x, (int)nextCell.y].direction != new Vector2(0, 0))
                {
                    RemoveWalls();
                    cell.direction = nextDirection;
                    Debug.Log(randomDirection);
                }

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