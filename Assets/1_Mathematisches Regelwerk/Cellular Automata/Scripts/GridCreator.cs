using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridCreator : MonoBehaviour
{
    public static GridCreator Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    
    [Header("Cellular Automata Probabilitys")]
    public int StartingProbability;
    public int BurningProbability;
    
    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject gridPrefab;
    
    Cell[,] grid; // ein array aus Zellen
    private void Start()
    {
        GenerateGrid();
    }
    
    public void GenerateGrid()
    {
        grid = new Cell[width, height]; //  erstellt ein 3D Grid aus Zellen
        
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // erstellt neues Zellen GameObject und gibt eine random Nummer
                    GameObject cellGO = Instantiate(cellPrefab); 
                    int random = Random.Range(0, 100);
                    
                    cellGO.transform.position = new Vector3(-width / 2 + x,0, -height / 2 + y);
                    grid[x, y] = cellGO.GetComponent<Cell>(); // Holt den "Cell" Component vom neuen GameObject 
                    // Debug.Log("Cell position = " + cellGO.transform.position);
                    //grid[x, y, z].SetState(Cell.States.Fresh);
                    
                    // Wenn die Zelle sich im inneren Bereich befindet, soll sie anfangen zu brennen.
                    if (x <= width * 0.5f + 2 && x >= width * 0.5f - 2 && y <= height * 0.5f + 2 && y >= height * 0.5f - 2)
                        grid[x, y].SetState(Cell.States.Burning);
                    
                    // Wenn die Zelle nicht im inneren ist, ist sie einfach Luft.
                    else if (random < 80)
                        grid[x, y].SetState(Cell.States.Air);
                    else
                        grid[x, y].SetState(Cell.States.Empty); // ? was ist fresh
                }
            }
        
        InstantiateGridContainer();
    }
    public void DeleteGrid()
    {
        if (grid == null)
            return;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y].DeleteCell();
            }
        }
    }
    
    public void InstantiateGridContainer()
    {
        GameObject gridGO = Instantiate(gridPrefab);
        GridContainer gridContainer = gridGO.GetComponent<GridContainer>();
        gridContainer.SetGrid(grid);
        StartCoroutine(gridContainer.UpdateGrid());
    }
}
