using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{

    public int sizeX, sizeY;
    public float generateDelay;
    public bool mazePlayable = false;
    public Cell cell;

    private Cell[,] Cells;

    private List<Cell> allCells;

    //A path can be made in 4 directions. in this case - up, right, down, left.
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

    public void CreateGrid()
    {
        //Instantiate the Grid
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                CreateCell(x, y);

            }
        }
        //Remove the top left and bottem right walls so we have a start and a finish for our maze.
        Cells[0, sizeY - 1].northWall.SetActive(false);
        Cells[sizeX - 1, 0].southWall.SetActive(false);
        
        //Add a finish and boxcollider with trigger to the component so we can really finish the maze.
        Cells[sizeX - 1, 0].gameObject.AddComponent<Finish>();
        Cells[sizeX - 1, 0].gameObject.AddComponent<BoxCollider>().isTrigger = true;

        StartCoroutine(RandomMazeWalker());
    }

    void CreateCell(int posX, int posY)
    {
        //Create a new cell on position and add it to the cells list so we can keep track
        Cell newCell = Instantiate(cell) as Cell;
        Cells[posX, posY] = newCell;
        newCell.transform.localPosition = new Vector3(posX, posY, 0);
        newCell.name = "Floor " + posX + " " + posY;
        newCell.transform.parent = this.transform;
        allCells.Add(newCell);

    }

    void RandomNextStep()
    {
        // set a new random direction, can't be the same as the previous direction
        randomDirection = Random.Range(0, 4);
        if (randomDirection == randomLastStep)
        {
            RandomNextStep();
            return;
        }

        randomLastStep = randomDirection;
        nextDirection = directions[randomDirection];
        
        //Get the nextcell position from our current cell + the next direction.
        nextCellPos = currentCellPos + nextDirection;

        CheckBounds();
    }


    public IEnumerator RandomMazeWalker()
    {
        //Random maze walker starts the maze untill it fails to find a new cell without a direction.
        for (failedTries = 0; failedTries <= 10; failedTries++)
        {
            RandomNextStep();

            //if the next cell doesn't have a direction and has atleast 3 walls we can go here
            if (Cells[(int)nextCellPos.x, (int)nextCellPos.y].direction == new Vector2(0, 0) && Cells[(int)nextCellPos.x, (int)nextCellPos.y].GetComponentsInChildren<Transform>().GetLength(0) > 2)
            {
                RemoveWalls();
                
                //Set the direction of the current cell so it cannot be visited again          
                Cells[(int)currentCellPos.x, (int)currentCellPos.y].direction = nextDirection;
                
                failedTries = 0;
                
                //Remove the cell from the list so we dont have to check it again
                allCells.Remove(Cells[(int)currentCellPos.x, (int)currentCellPos.y]);
            }

            yield return new WaitForSeconds(generateDelay);


        }
        StartCoroutine(FinishMaze());
    }

    void CheckBounds()
    {
        //Check if the next position is within the array, if it is not try a new random direction
        if (nextCellPos.x < sizeX && nextCellPos.x >= 0 && nextCellPos.y < sizeY && nextCellPos.y >= 0)
        {
            return;
        }
        else
            RandomNextStep();
    }

    IEnumerator FinishMaze()
    {
        //Run this untill there are no cells left that do not have a direction.
        while (noDirectionCount > 0)
        {
            noDirectionCount = 0;
            //Start at the back of the List so we can remove a cell from it if it has a direction
            for (int i = allCells.Count - 1; i > -1; i--)
            {
                if (allCells[i].direction == new Vector2(0, 0) && allCells[i].GetComponentsInChildren<Transform>().GetLength(0) > 2)
                {
                    //Set the currentCellPos to the current allCell index so that we can remove its walls if it needs to
                    currentCellPos = new Vector2((int)allCells[i].transform.position.x,(int)allCells[i].transform.position.y);

                    RandomNextStep();

                    //cell has no direction yet so increase the count
                    noDirectionCount++;

                    //Check if nextCell already has a direction if so we add the current cell to the path.
                    if (Cells[(int)nextCellPos.x, (int)nextCellPos.y].direction != new Vector2(0, 0) && Cells[(int)nextCellPos.x, (int)nextCellPos.y].GetComponentsInChildren<Transform>().GetLength(0) > 2)
                    {
                        //Cell is getting a direction so decrease the count
                        noDirectionCount--;

                        RemoveWalls();
                        
                        allCells[i].direction = nextDirection;

                        //Remove the cell from the list because it now has a direction
                        allCells.RemoveAt(i);
                        yield return new WaitForSeconds(generateDelay);
                    }
                }
            }
        }
        //If this method has finished we can play the maze
        mazePlayable = true;
    }

    //Return a cell with a position so we can acces cell properties easily.
    Cell GetCell(Vector2 cellPos)
    {
        return Cells[(int)cellPos.x, (int)cellPos.y];
    }

    void RemoveWalls()
    {
        //Create a path by destroying walls on both sides.

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
        //if we have removed the walls we move to the next cell.
        currentCellPos = nextCellPos;
    }

}