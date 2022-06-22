using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    public SquareGrid squareGrid;
    
    public void GenerateMesh(int[,] map, float squareSize)
    {
        squareGrid = new SquareGrid(map, squareSize);
    }
    private void OnDrawGizmos()
    {
        if (squareGrid != null)
        {
            for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
            {
                for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
                {
                    Gizmos.color = squareGrid.squares[x, y].topLeft.active ? Color.black : Color.white; 
                    Gizmos.DrawCube(squareGrid.squares[x,y].topLeft.position, Vector3.one  * .4f);
                    
                    Gizmos.color = squareGrid.squares[x, y].topRight.active ? Color.black : Color.white; 
                    Gizmos.DrawCube(squareGrid.squares[x,y].topRight.position, Vector3.one  * .4f);
                    
                    Gizmos.color = squareGrid.squares[x, y].bottomLeft.active ? Color.black : Color.white; 
                    Gizmos.DrawCube(squareGrid.squares[x,y].bottomLeft.position, Vector3.one  * .4f);
                    
                    Gizmos.color = squareGrid.squares[x, y].bottomRight.active ? Color.black : Color.white; 
                    Gizmos.DrawCube(squareGrid.squares[x,y].bottomRight.position, Vector3.one  * .4f);

                    Gizmos.color = Color.grey;
                    Gizmos.DrawCube(squareGrid.squares[x,y].centreTop.position, Vector3.one  * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x,y].centreRight.position, Vector3.one  * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x,y].centreLeft.position, Vector3.one  * .15f);
                    Gizmos.DrawCube(squareGrid.squares[x,y].centreBottom.position, Vector3.one  * .15f);
                } 
            }
        }
    }
    public class SquareGrid
    {
        public Square[,] squares;

        // Square Grid Konstruktor
        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0); // -> 0 gibt die Breite des Arrays
            int nodeCountY = map.GetLength(1); // -> 1 gibt die Höhe des Arrays

            // Kalkuliert die Grösse der gesamten Map
            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            
            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];
            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 position = new Vector3(-mapWidth/2 + x * squareSize + squareSize/2, 0, -mapHeight/2 + y * squareSize + squareSize/2);
                    controlNodes[x, y] = new ControlNode(position, map[x, y] == 1, squareSize);
                }
            }

            squares = new Square[nodeCountX -1, nodeCountY -1];
            for (int x = 0; x < nodeCountX -1; x++)
            {
                for (int y = 0; y < nodeCountY -1; y++)
                {
                    squares[x, y] = new Square(controlNodes[x, y + 1], 
                                                controlNodes[x + 1, y + 1],
                                                controlNodes[x + 1, y],
                                                controlNodes[x, y]);
                }
            }
        }
    }
    
    public class Square
    {
        public ControlNode topLeft, topRight, bottomLeft, bottomRight;
        public Node centreTop, centreRight, centreBottom, centreLeft;

        // Konstruktor von einem Square mit allen Nodes
        public Square(ControlNode _topLeft, ControlNode _topRight, ControlNode _bottomLeft, ControlNode _bottomRight)
        {
            // Control Nodes
            topLeft = _topLeft;
            topRight = _topRight;
            bottomLeft = _bottomLeft;
            bottomRight = _bottomRight;

            // Für die Nodes und zu welcher Control Node sie gehören.
            centreTop = topLeft.right;
            centreBottom = bottomLeft.right;
            centreLeft = bottomLeft.above;
            centreRight = bottomRight.above;
        }
    }

    public class Node
    {
        public Vector3 position;
        public int vertexIndex = -1;

        // Konstruktor für eine Node
        public Node(Vector3 _pos)
        {
            position = _pos;
        }
    }
    
    public class ControlNode : Node
    {
        // Jeder der Würfel hat 4 Control Nodes die an den Kanten verteilt sind.
        // -> Jeder dieser Control Node von einem Würfel hat 2 Nodes die ihm untergeordnet sind -> oben & rechts.
        public bool active;
        public Node above, right; 

        public ControlNode(Vector3 _pos, bool _active, float sqaureSize) : base(_pos)
        {
            active = _active;
            // Erstellt eine neue Node und positioniert sie an der richtigen Stelle(Kante) mit einem Vektor
            // der halb so lang ist wie das Viereck. 
            above = new Node(position + Vector3.forward * sqaureSize / 2f);
            right = new Node(position + Vector3.right * sqaureSize / 2f);
        }
    }
    
}
