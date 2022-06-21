using System;
using System.Collections;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)] public int RandomFillPercent;

    int[,] map;

    // [SerializeField] private GameObject cubePrefab;
    private void Start()
    {
        GenerateMap();
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GenerateMap(); 
        }
    }

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for (int i = 0; i < 5; i++)
        {
            SmoothMap();
        }
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }

        System.Random pseudoRandomNumber = new System.Random(seed.GetHashCode());
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    // goes trough the int 2D Map array and gives a "random" number. Then checks if its higher or lower than the random fill percent.
                    map[x, y] = pseudoRandomNumber.Next(0, 100) < RandomFillPercent ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighbourWallTiles = GetSurroundingWallCount(x, y);

                if (neighbourWallTiles > 4)
                    map[x, y] = 1;
                else if(neighbourWallTiles < 4)
                {
                    map[x, y] = 0;
                }
            }
        }
    }

    
    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        // Loops trough a 3x3 grid -> gridX and gridY
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                // Check if grid is inside the map
                if (neighbourX >= 0 && neighbourX <= width && neighbourY >= 0 && neighbourY <= width)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else // if grid is outside of the map
                {
                    wallCount += 1;
                }
                
            } 
        }

        return wallCount;
    }

    void OnDrawGizmos()
    {
        if (map != null)
        {
            for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                // checks if the result of the values in the 2D array was 1 or 0 and gives the right color.
                Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;

                Vector3 pos = new Vector3(-width / 2 + x + 0.5f, 0, -height / 2 + y + 0.5f);

                Gizmos.DrawCube(pos, Vector3.one);
                //Instantiate(cubePrefab, pos, Quaternion.identity);
            }
        }
    }
}