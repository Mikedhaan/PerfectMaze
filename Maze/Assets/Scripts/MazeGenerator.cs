using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

    public int sizeX, sizeY;
    public float generateDelay;

    public Cell cell;

    private Cell[,] Cells;

    private List<Cell> allCells;

    private Vector2[] directions =
        {
            new Vector2 (0, 1),
            new Vector2(1, 0),
            new Vector2(0,-1),
            new Vector2(-1, 0)
        };
    private Vector2 nextDirection;
    private Vector2 currentCellPos;
    private Vector2 nextCellPos;

    private int randomLastStep;
    private int randomDirection;
    private int failedTries;
    private int noDirectionCount = 1;

    // Use this for initialization
    void Start()
    {
        Cells = new Cell[sizeX, sizeY];
        allCells = new List<Cell>();
        currentCellPos = new Vector2((int)sizeX / 2, (int)sizeY / 2);
        randomLastStep = 0;
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void RandomNextStep()
    {
        randomDirection = Random.Range(0, 4);
        if (randomDirection == randomLastStep)
        {
            RandomNextStep();
            return;
        }

        randomLastStep = randomDirection;
        nextDirection = directions[randomDirection];
        
        nextCellPos = currentCellPos + nextDirection;

        CheckBounds();
    }

    public void CreateGrid()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                CreateCell(x, y);
                
            }
        }
        Cells[0, sizeY-1].northWall.SetActive(false);
        Cells[sizeX-1, 0].southWall.SetActive(false);
        StartCoroutine(RandomMazeWalker());
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

    public IEnumerator RandomMazeWalker()
    {
        for (failedTries = 0; failedTries <= 10; failedTries++)
        {
            RandomNextStep();

            if (Cells[(int)nextCellPos.x, (int)nextCellPos.y].direction == new Vector2(0, 0) && Cells[(int)nextCellPos.x, (int)nextCellPos.y].GetComponentsInChildren<Transform>().GetLength(0) > 2)
            {
                RemoveWalls();

                Cells[(int)currentCellPos.x, (int)currentCellPos.y].direction = nextDirection;
                failedTries = 0;
                allCells.Remove(Cells[(int)currentCellPos.x, (int)currentCellPos.y]);
            }

            yield return new WaitForSeconds(generateDelay);


        }
        StartCoroutine(FinishMaze());
    }

    void CheckBounds()
    {
        if (nextCellPos.x < sizeX && nextCellPos.x >= 0 && nextCellPos.y < sizeY && nextCellPos.y >= 0)
        {
            return;
        }
        else
            RandomNextStep();
    }

    IEnumerator FinishMaze()
    {
        while (noDirectionCount > 0)
        {
            noDirectionCount = 0;
            for (int i = allCells.Count - 1; i > -1; i--)
            {
                if (allCells[i].direction == new Vector2(0, 0) && allCells[i].GetComponentsInChildren<Transform>().GetLength(0) > 2)
                {
                    currentCellPos = new Vector2((int)allCells[i].transform.position.x,(int)allCells[i].transform.position.y);
                    RandomNextStep();
                    noDirectionCount++;

                    if (Cells[(int)nextCellPos.x, (int)nextCellPos.y].direction != new Vector2(0, 0) && Cells[(int)nextCellPos.x, (int)nextCellPos.y].GetComponentsInChildren<Transform>().GetLength(0) > 2)
                    {
                        RemoveWalls();
                        noDirectionCount--;
                        allCells[i].direction = nextDirection;
                        allCells.RemoveAt(i);
                        yield return new WaitForSeconds(generateDelay);
                    }
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
            GetCell(currentCellPos).northWall.SetActive(false);
            GetCell(nextCellPos).southWall.SetActive(false);
        }
        if (randomDirection == 1)
        {
            //right
            GetCell(currentCellPos).eastWall.SetActive(false);
            GetCell(nextCellPos).westWall.SetActive(false);
        }
        if (randomDirection == 2)
        {
            //down
            GetCell(currentCellPos).southWall.SetActive(false);
            GetCell(nextCellPos).northWall.SetActive(false);
        }
        if (randomDirection == 3)
        {
            //left
            GetCell(currentCellPos).westWall.SetActive(false);
            GetCell(nextCellPos).eastWall.SetActive(false);
        }

        currentCellPos = nextCellPos;
    }

}