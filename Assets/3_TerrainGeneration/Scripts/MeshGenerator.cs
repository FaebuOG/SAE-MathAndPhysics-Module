using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator 
{
   public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve)
   {
      int width = heightMap.GetLength(0);
      int height = heightMap.GetLength(1);
      float topLeftX = (width - 1) / -2f;
      float topLeftZ = (height - 1) / 2f;

      MeshData meshData = new MeshData(width, height);
      int vertexIndex = 0;

      for (int y = 0; y < height; y++)
      {
         for (int x = 0; x < width; x++)
         {
            meshData.Vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap [x, y]) * heightMultiplier, topLeftZ - y);
            meshData.UVs[vertexIndex] = new Vector2(x / (float) width, y/(float)height);
            
            if (x < width - 1 && y < height - 1)
            {
               
               meshData.AddTriangles(vertexIndex, vertexIndex + width + 1, vertexIndex + width); // 1. Triangle
               meshData.AddTriangles(vertexIndex + width + 1, vertexIndex, vertexIndex + 1); // 2. Triangle
            }
            
            vertexIndex++;
         }
      }

      return meshData;
   }
}

public class MeshData
{
   public Vector3[] Vertices;
   public Vector2[] UVs;
   private int[] triangles;
   private int triangleIndex;
   
   // gets the correct mesh data based on given width and height to CreateMesh()
   public MeshData(int meshWidth, int meshHeight)
   {
      Vertices = new Vector3[meshWidth * meshHeight];
      UVs = new Vector2[meshHeight * meshHeight];
      triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6]; // count of all the triangles
   }

   public void AddTriangles(int a, int b, int c)
   {
      triangles[triangleIndex] = a;
      triangles[triangleIndex + 1] = b;
      triangles[triangleIndex + 2] = c;
      triangleIndex += 3;
   }
   
   public Mesh CreateMesh()
   {
      Mesh mesh = new Mesh();
      mesh.vertices = Vertices;
      mesh.triangles = triangles;
      mesh.uv = UVs;
      mesh.RecalculateNormals();
      return mesh;
   }
}
