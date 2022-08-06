using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridContainer : MonoBehaviour
{
    [Header("Array Values")]
    [SerializeField] GameObject cellPrefab;
    [SerializeField] int height;
    [SerializeField] int width;

    [Header("Probabilitys")]
    [SerializeField] private int startingProbability;
    [SerializeField] private int burningProbability;
    Cell[,] grid;
    
    // Coroutine Methode wo die Bedingungen für die Zellulären Automaten abgefragt werden.
    // -> Wird aktuell 10x pro Sekunde ge-updated.
    public IEnumerator UpdateGrid()
    {
        bool keepUpdating = true;
        int amountOfBurnedCells = 0;
        while (keepUpdating)
        {
            if (grid == null)
                break;
            
            Cell[,] updatedGrid = grid;
            int random;
            
                for (int heightY = 0; heightY < height; heightY++)
                {
                    for (int widthX = 0; widthX < width; widthX++)
                    {
                        switch (grid[widthX,heightY].State)
                            {
                                case Cell.States.Empty:
                                    break;
                                case Cell.States.Starting:
                                    random = Random.Range(0, 100);
                                    // A small fire appears on the cell with a chance to make current cell switch from state "Starting" to state "Burning".
                                    if (random < GridCreator.Instance.StartingProbability)
                                        updatedGrid[widthX, heightY].SetState(Cell.States.Burning);
                                    break;
                                case Cell.States.Burning:
                                    random = Random.Range(0, 100);
                                    //50% Chance to make a neighbouring cell switch to state "Starting" if it's state is "Empty".
                                    if (random < GridCreator.Instance.BurningProbability)
                                        for (int neighbourY = heightY - 2; neighbourY < heightY + 3; neighbourY++) // neighbour cells not working propperly yet
                                        {
                                            for (int neighbourX = widthX - 2; neighbourX < widthX + 3; neighbourX++)
                                            {
                                                
                                                
                                                
                                                if (!(neighbourY == heightY && neighbourX == widthX || neighbourY < 0 || neighbourY > height - 1 || neighbourX < 0 || neighbourX > width - 1))
                                                {
                                                    if (grid[neighbourX, neighbourY].State == Cell.States.Empty)
                                                    {
                                                        updatedGrid[neighbourX, neighbourY].SetState(Cell.States.Starting);
                                                    }
                                                }
                                            }
                                        }

                                    //for (int x = 1; x < width-1; x++)
                                    //{
                                    //    for (int y = 1; y < height-1; y++)
                                    //    {
                                    //        int neighbours = 0;

                                    //        // Add up all the states in a 3x3 grid
                                    //        for (int i = -1; i <= 1; i++)
                                    //        {
                                    //            for (int j = -1; j <= 1; j++)
                                    //            {
                                    //                neighbours += grid[x+i, y+j];
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    
                                    if (Time.time - grid[widthX, heightY].TimeWhenStartedBurning < 10)
                                        continue;
                                    //Make the current cell switch from state "Burning" to state "Burned" after 10 seconds.
                                    updatedGrid[widthX, heightY].SetState(Cell.States.Burned);
                                    amountOfBurnedCells++;
                                    break;
                            }

                    }
                }

                grid = updatedGrid;
                if (amountOfBurnedCells == grid.Length)
                    keepUpdating = false;
            // yield return new WaitForSeconds(0.1f);
            yield return new WaitForSeconds(.5f);
        }
    }

    // Wejst die richtigen Variablen den richtigen Dimensionen des Grids zu, damit wir es mit Parametern leicht benutzen können.
    public void SetGrid(Cell[,] cellGrid)
    {
        grid = cellGrid;
        height = grid.GetLength(0);
        width = grid.GetLength(1);
    }

}
